#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace IAPToolkit.EditorUI
{
    /// <summary>
    /// Provides preconfigured GUIStyles for fonts used in the IAP Toolkit editor UI.
    /// </summary>
    public static class IAPEditorFonts
    {
        /// <summary>
        /// Large, bold, color-accented section title.
        /// </summary>
        public static GUIStyle Title => new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            normal = { textColor = IAPEditorColors.TitleText }
        };

        /// <summary>
        /// Bold red warning/error label.
        /// </summary>
        public static GUIStyle Warning => new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Bold,
            normal = { textColor = IAPEditorColors.WarningText }
        };

        /// <summary>
        /// Small, gray informational label (e.g. hints, inline notes).
        /// </summary>
        public static GUIStyle SmallGray => new GUIStyle(EditorStyles.label)
        {
            fontSize = 10,
            normal = { textColor = Color.gray }
        };
    }
}
#endif