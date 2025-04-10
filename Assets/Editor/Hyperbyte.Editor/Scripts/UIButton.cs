namespace Hyperbyte.EditorUtil
{
    using UnityEngine;
    using System;

    public class UIButton : EditorUIElement
    {
        public string Label { get; private set; }
        private readonly Action onClick;

        public UIButton(string label, Action onClick, GUIStyle style)
        {
            Label = label;
            this.onClick = onClick;
            Style = style;
        }

        public override void Draw()
        {
            if (GUILayout.Button(Label, Style))
                onClick?.Invoke();
        }
    }
}