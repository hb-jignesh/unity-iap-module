namespace Hyperbyte.EditorUtil
{
    using UnityEngine;
    using System;

    public static class EditorUIFactory
    {
        public static UIButton Button(string label, Action onClick)
            => new UIButton(label, onClick, EditorStyleManager.ButtonStyle);

        public static UILabel Label(string content)
            => new UILabel(content, EditorStyleManager.LabelStyle);

        public static UIToggle Toggle(string label, bool initialValue, Action<bool> onValueChanged)
            => new UIToggle(label, initialValue, onValueChanged, EditorStyleManager.ToggleStyle);
    }
}