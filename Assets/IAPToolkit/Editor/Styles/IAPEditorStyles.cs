#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace IAPToolkit.EditorUI
{
    /// <summary>
    /// Provides reusable editor GUI layout styles and utilities for drawing custom UI elements.
    /// </summary>
    public static class IAPEditorStyles
    {
        private static Texture2D _cardBackgroundTexture;

        /// <summary>
        /// GUIStyle for boxed card layout (with optional background texture).
        /// </summary>
        public static GUIStyle CardBox
        {
            get
            {
                var style = new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(10, 10, 10, 10),
                    margin = new RectOffset(6, 6, 6, 6)
                };

                // Use background texture if available
                if (CardBackgroundTexture != null)
                    style.normal.background = CardBackgroundTexture;

                return style;
            }
        }

        /// <summary>
        /// Draws a flat colored rectangle background using the current card color.
        /// </summary>
        /// <param name="rect">Target rect to fill.</param>
        public static void DrawCardBackground(Rect rect)
        {
            EditorGUI.DrawRect(rect, IAPEditorColors.CardBackground);
        }

        /// <summary>
        /// Optional background texture for custom GUI cards.
        /// Should be placed at: Assets/IAPToolkit/Editor/Resources/card-bg.png
        /// </summary>
        private static Texture2D CardBackgroundTexture
        {
            get
            {
                if (_cardBackgroundTexture == null)
                    _cardBackgroundTexture = Resources.Load<Texture2D>("card-bg");
                return _cardBackgroundTexture;
            }
        }
    }
}
#endif