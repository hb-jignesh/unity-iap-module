#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace IAPToolkit
{
    [CustomEditor(typeof(IAPProductConfig))]
    public class IAPProductConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(
                "This is the internal product config file for IAP Toolkit.\n\nPlease use the 'Hyperbyte > IAPToolkit > Configure' window to manage products.",
                MessageType.Info
            );

            GUILayout.Space(10);

            if (GUILayout.Button("Open IAP Configuration Window", GUILayout.Height(30)))
            {
                IAPProductManagerWindow.ShowWindow();
            }
        }
    }
}
#endif