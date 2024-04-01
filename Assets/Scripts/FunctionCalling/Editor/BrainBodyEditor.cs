using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Indie.Editor
{
    [CustomEditor(typeof(BrainBody))]
    public class BrainBodyEditor : UnityEditor.Editor
    {
        private Texture2D headerImage;
        private GUIStyle titleStyle;

        private Color backgroundColor = new Color(0, 0, 0);
        private Color buttonColor = new Color(0.2f, 0.4f, 0.6f); // Example button color


        private void OnEnable()
        {
            // Load header image
            headerImage = Resources.Load<Texture2D>("Textures/Anger");

            // Create custom title style
            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontSize = 20;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
        }

        public override void OnInspectorGUI()
        {
            // Begin a vertical area with a background color
            EditorGUILayout.BeginVertical(GUI.skin.box);

            DrawHeader();


            DrawDefaultInspector();

            DrawButtons();

            EditorGUILayout.EndVertical();

            // Apply the button style
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.hover.textColor = Color.yellow;
            buttonStyle.active.textColor = Color.green;
            buttonStyle.normal.background = EditorGUIUtility.whiteTexture;

            // Apply custom background color to buttons
            GUI.backgroundColor = buttonColor;

            // Draw custom buttons
            GUILayout.Space(10);
            if (GUILayout.Button("Forget Conversation"))
            {
                // Call ForgetConversation method
                ((BrainBody)target).ForgetConversation();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Forget Context"))
            {
                // Call ForgetContext method
                ((BrainBody)target).ForgetContext();
            }

            // Reset the GUI background color to its default
            GUI.backgroundColor = Color.white;

        }

        private void DrawHeader()
        {
            // Draw header image
            GUILayout.Space(20);
            Rect rect = EditorGUILayout.GetControlRect(false, 100f);
            EditorGUI.DrawTextureTransparent(rect, headerImage, ScaleMode.ScaleToFit);

            // Draw title
            GUILayout.Space(10);
            //EditorGUILayout.LabelField("BrainBody Settings", titleStyle);
            GUILayout.Space(10);
        }


        private void DrawButtons()
        {
            GUILayout.Space(20);

            // Get the target script
            BrainBody brainBody = (BrainBody)target;

            // Draw buttons for public methods
            if (GUILayout.Button("Start Listening"))
            {
                brainBody.StartListening();
            }

            if (GUILayout.Button("Stop Listening"))
            {
                brainBody.StopListening();
            }

            if (GUILayout.Button("Understand Speech"))
            {
                brainBody.UnderstandSpeech();
            }

            if (GUILayout.Button("Read Text"))
            {
                brainBody.ReadText();
            }

            if (GUILayout.Button("Text Response"))
            {
                brainBody.TextResponse();
            }

            if (GUILayout.Button("Voice Response"))
            {
                brainBody.VoiceResponse();
            }

            if (GUILayout.Button("Act"))
            {
                brainBody.Act();
            }

            if (GUILayout.Button("Action Chat"))
            {
                brainBody.ActionChat();
            }

            if (GUILayout.Button("Forget Conversation"))
            {
                brainBody.ForgetConversation();
            }

            if (GUILayout.Button("Forget Context"))
            {
                brainBody.ForgetContext();
            }

            GUILayout.Space(20);
        }
    }
}
