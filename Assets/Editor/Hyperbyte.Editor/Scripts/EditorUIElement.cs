namespace Hyperbyte.EditorUtil
{
    using UnityEngine;

    public abstract class EditorUIElement : IEditorUIElement
    {
        public GUIStyle Style { get; protected set; }

        public abstract void Draw();
    }
}