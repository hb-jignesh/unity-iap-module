#if UNITY_EDITOR
using UnityEngine;

namespace IAPToolkit.EditorUI
{
    /// <summary>
    /// Provides theme-aware color constants for styling the IAP Toolkit editor UI.
    /// </summary>
    public static class IAPEditorColors
    {
        /// <summary>
        /// Background color for UI cards.
        /// </summary>
        public static Color CardBackground =>
            IAPEditorTheme.IsDarkTheme ? new Color(0.17f, 0.19f, 0.22f) : new Color(0.95f, 0.97f, 1f);

        /// <summary>
        /// Primary accent color for section titles.
        /// </summary>
        public static Color TitleText =>
            IAPEditorTheme.IsDarkTheme ? new Color(0.8f, 0.9f, 1f) : new Color(0.2f, 0.4f, 0.8f);

        /// <summary>
        /// Warning or error text color.
        /// </summary>
        public static Color WarningText => new Color(1f, 0.3f, 0.3f);
    }
}
#endif