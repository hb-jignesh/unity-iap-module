namespace Hyperbyte.EditorUtil
{
    using UnityEngine;

    public static class EditorStyleManager
    {
        private static bool initialized = false;

        public static GUIStyle ButtonStyle;
        public static GUIStyle LabelStyle;
        public static GUIStyle ToggleStyle;

        // Call this explicitly from OnGUI()
        public static void Initialize()
        {
            if (initialized) return;

            ButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(8, 8, 4, 4),
                normal = { textColor = Color.white },
                hover = { textColor = Color.green }
            };

            LabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Normal,
                richText = true
            };

            ToggleStyle = new GUIStyle(GUI.skin.toggle)
            {
                fontSize = 12,
                fontStyle = FontStyle.Normal
            };

            initialized = true;
        }
    }
}