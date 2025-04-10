namespace Hyperbyte.EditorUtil
{
    using UnityEditor;
    using UnityEngine;

    public abstract class EditorUtilWindow : EditorWindow
    {
        protected virtual void OnEnable()
        {
            EditorStyleManager.Initialize();
        }

        protected bool Button(string label, GUIStyle style = null, params GUILayoutOption[] options)
        {
            return GUILayout.Button(label, style ?? EditorStyleManager.ButtonStyle, options);
        }

        protected void Label(string content, GUIStyle style = null, params GUILayoutOption[] options)
        {
            GUILayout.Label(content, style ?? EditorStyleManager.LabelStyle, options);
        }

        protected bool Toggle(string label, bool value, GUIStyle style = null, params GUILayoutOption[] options)
        {
            return GUILayout.Toggle(value, label, style ?? EditorStyleManager.ToggleStyle, options);
        }

        // Similarly, you can add more helper methods like TextField, IntField, FloatField, etc.
    }
}