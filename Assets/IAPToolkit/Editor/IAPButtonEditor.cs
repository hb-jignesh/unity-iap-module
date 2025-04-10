#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace IAPToolkit
{
    [CustomEditor(typeof(IAPButton))]
    public class IAPButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var button = (IAPButton)target;

            EditorGUILayout.LabelField("IAP Button", EditorStyles.boldLabel);

            IAPProductConfig config = Resources.Load<IAPProductConfig>("IAPProductConfig");

            if (config != null && config.products != null && config.products.Length > 0)
            {
                string[] keys = System.Array.ConvertAll(config.products, p => p.key);
                int currentIndex = Mathf.Max(0, System.Array.IndexOf(keys, button.productKey));

                int selectedIndex = EditorGUILayout.Popup("Product Key", currentIndex, keys);
                button.productKey = keys[selectedIndex];
            }
            else
            {
                EditorGUILayout.HelpBox("IAPProductConfig not found or no products defined.\nYou can enter a key manually.", MessageType.Warning);
                button.productKey = EditorGUILayout.TextField("Product Key", button.productKey);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(button);
            }
        }
    }
}
#endif