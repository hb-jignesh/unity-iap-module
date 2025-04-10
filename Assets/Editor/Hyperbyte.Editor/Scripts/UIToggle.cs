namespace Hyperbyte.EditorUtil
{
    using UnityEngine;
    using System;

    public class UIToggle : EditorUIElement
    {
        public string Label { get; private set; }
        public bool Value { get; private set; }
        private readonly Action<bool> onValueChanged;

        public UIToggle(string label, bool initialValue, Action<bool> onValueChanged, GUIStyle style)
        {
            Label = label;
            Value = initialValue;
            this.onValueChanged = onValueChanged;
            Style = style;
        }

        public override void Draw()
        {
            bool newValue = GUILayout.Toggle(Value, Label, Style);
            if (newValue != Value)
            {
                Value = newValue;
                onValueChanged?.Invoke(Value);
            }
        }
    }
}