#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using IAPToolkit.EditorUI;

namespace IAPToolkit
{
    public class IAPProductManagerWindow : EditorWindow
    {
        private IAPProductConfig config;
        private const string configPath = "Assets/IAPToolkit/Resources/IAPProductConfig.asset";

        [MenuItem("Hyperbyte/IAPToolkit/Configure")]
        public static void ShowWindow()
        {
            GetWindow<IAPProductManagerWindow>("IAP Toolkit Config");
        }

        private void OnEnable()
        {
            config = LoadOrCreateConfig();
            if (config.products == null)
                config.products = new IAPProductEntry[0];
        }

        private void OnGUI()
{
    if (config == null)
    {
        EditorGUILayout.HelpBox("Could not load IAPProductConfig.", MessageType.Error);
        return;
    }

    EditorGUILayout.Space();

    var list = new List<IAPProductEntry>(config.products);

    // 🧼 Track what to modify after drawing
    int? addAfterIndex = null;
    int? removeAtIndex = null;

    if (list.Count == 0)
    {
        EditorGUILayout.HelpBox("No products yet. Add your first one below.", MessageType.Info);
    }

    for (int i = 0; i < list.Count; i++)
    {
        var entry = list[i];
        string label = string.IsNullOrWhiteSpace(entry.key) ? $"Product {i + 1}" : $"[{i + 1}] {entry.key}";
        bool isExpanded = GetFoldoutState(entry.key);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        isExpanded = EditorGUILayout.Foldout(isExpanded, label, true);
        SetFoldoutState(entry.key, isExpanded);

        if (isExpanded)
        {
            EditorGUI.indentLevel++;

            bool isDuplicate = false;
            for (int j = 0; j < list.Count; j++)
            {
                if (j != i && list[j].key == entry.key)
                {
                    isDuplicate = true;
                    break;
                }
            }

            GUI.color = isDuplicate ? Color.red : Color.white;
            entry.key = EditorGUILayout.TextField("Key", entry.key);
            GUI.color = Color.white;

            if (isDuplicate)
                EditorGUILayout.LabelField("Duplicate key!", IAPEditorFonts.Warning);

            entry.overrideStoreIds = EditorGUILayout.Toggle("Override Product ID per Store", entry.overrideStoreIds);

            if (entry.overrideStoreIds)
            {
                entry.androidProductId = EditorGUILayout.TextField("Android Product ID", entry.androidProductId);
                entry.iosProductId = EditorGUILayout.TextField("iOS Product ID", entry.iosProductId);
            }
            else
            {
                entry.productId = EditorGUILayout.TextField("Product ID", entry.productId);
            }

            entry.productType = (IAPProductType)EditorGUILayout.EnumPopup("Product Type", entry.productType);
            entry.price = EditorGUILayout.TextField("Price (optional)", entry.price);

            EditorGUILayout.Space(6);

            // 🔘 Action Bar: Up/Down (left), Add/Remove (right)
            EditorGUILayout.BeginHorizontal();

            // ← Up/Down
            EditorGUILayout.BeginHorizontal(GUILayout.Width(80));
            GUI.enabled = i > 0;
            if (GUILayout.Button("↑", GUILayout.Width(30))) Swap(list, i, i - 1);
            GUI.enabled = i < list.Count - 1;
            if (GUILayout.Button("↓", GUILayout.Width(30))) Swap(list, i, i + 1);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            // → Add/Remove (deferred)
            EditorGUILayout.BeginHorizontal(GUILayout.Width(80));
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                addAfterIndex = i;
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                removeAtIndex = i;
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(4);
    }

    // 🔁 Apply changes outside GUI drawing loop
    if (removeAtIndex.HasValue && removeAtIndex.Value < list.Count)
        list.RemoveAt(removeAtIndex.Value);

    if (addAfterIndex.HasValue)
    {
        var newEntry = new IAPProductEntry
        {
            key = $"NewProduct{Random.Range(1000, 9999)}",
            productId = "com.company.newproduct",
            productType = IAPProductType.Consumable,
            price = "$0.99"
        };
        list.Insert(addAfterIndex.Value + 1, newEntry);
    }

    // 💾 Save
    config.products = list.ToArray();
    EditorUtility.SetDirty(config);

    // ➕ Full-width Add button (bottom if empty)
    GUILayout.FlexibleSpace();
    if (list.Count == 0)
    {
        GUILayout.Space(12);
        if (GUILayout.Button("➕ Add New Product", GUILayout.Height(40)))
        {
            var newEntry = new IAPProductEntry
            {
                key = $"NewProduct{Random.Range(1000, 9999)}",
                productId = "com.company.newproduct",
                productType = IAPProductType.Consumable,
                price = "$0.99"
            };
            config.products = new IAPProductEntry[] { newEntry };
        }
    }
}



        

        private void Swap(List<IAPProductEntry> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        private IAPProductConfig LoadOrCreateConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<IAPProductConfig>(configPath);

            if (config == null)
            {
                if (!AssetDatabase.IsValidFolder("Assets/IAPToolkit"))
                    AssetDatabase.CreateFolder("Assets", "IAPToolkit");

                if (!AssetDatabase.IsValidFolder("Assets/IAPToolkit/Resources"))
                    AssetDatabase.CreateFolder("Assets/IAPToolkit", "Resources");

                config = ScriptableObject.CreateInstance<IAPProductConfig>();
                AssetDatabase.CreateAsset(config, configPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            return config;
        }

        private bool GetFoldoutState(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return true;
            return EditorPrefs.GetBool("IAP_Foldout_" + key, true);
        }

        private void SetFoldoutState(string key, bool state)
        {
            if (!string.IsNullOrWhiteSpace(key))
                EditorPrefs.SetBool("IAP_Foldout_" + key, state);
        }
    }
}
#endif
