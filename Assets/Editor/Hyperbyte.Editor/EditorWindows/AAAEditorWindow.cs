namespace Hyperbyte.EditorUtil
{
    using UnityEditor;
    using UnityEngine;

    public class AAAEditorWindow : EditorUtilWindow
    {
        [MenuItem("Hyperbyte/Editor Utilities/AAA Editor")]
        public static void ShowWindow()
        {
            GetWindow<AAAEditorWindow>("AAA Editor");
        }

        private bool featureEnabled = false;

        protected override void OnEnable()
        {
            base.OnEnable(); // Ensures EditorStyleManager initialization
        }

        private void OnGUI()
        {
            Label("Welcome to AAA Editor!");

            if (Button("Click Me!"))
            {
                Debug.Log("Button Clicked!");
            }

            featureEnabled = Toggle("Enable Feature", featureEnabled);
        }
    }
}