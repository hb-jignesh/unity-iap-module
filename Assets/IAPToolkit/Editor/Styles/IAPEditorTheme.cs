#if UNITY_EDITOR
using UnityEditor;

namespace IAPToolkit.EditorUI
{
    /// <summary>
    /// Provides utility for checking the current Unity Editor theme (light or dark).
    /// </summary>
    public static class IAPEditorTheme
    {
        /// <summary>
        /// True if Unity is using the Pro (dark) theme.
        /// </summary>
        public static bool IsDarkTheme => EditorGUIUtility.isProSkin;
    }
}
#endif