using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TimelineViewer
{
    public class WarningPopup
#if UNITY_EDITOR
    : EditorWindow
#endif
    {
#if UNITY_EDITOR
        static string message;
        static string buttonText;
        public static void Init(string msg, string btext)
        {
            WarningPopup window = ScriptableObject.CreateInstance<WarningPopup>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);

            message = msg;
            buttonText = btext;

            window.ShowPopup();
        }

        private void OnDestroy()
        {
            message = "";
            buttonText = "";
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField(message, EditorStyles.wordWrappedLabel);
            GUILayout.Space(70);
            if (GUILayout.Button(buttonText)) this.Close();
        }
#endif
    }
}
