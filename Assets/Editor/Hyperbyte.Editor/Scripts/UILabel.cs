namespace Hyperbyte.EditorUtil
{
    using UnityEngine;

    public class UILabel : EditorUIElement
    {
        public string Content { get; private set; }

        public UILabel(string content, GUIStyle style)
        {
            Content = content;
            Style = style;
        }

        public override void Draw()
        {
            GUILayout.Label(Content, Style);
        }
    }
}