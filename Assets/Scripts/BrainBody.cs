using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Indie.OpenAI.Brain;
using Indie.Attributes;
using Indie.Voice;
using UnityEngine.UI;
using TMPro;
using Indie.OpenAI.API;
using System.Threading.Tasks;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;
using Doozy.Runtime.Common.Extensions;


namespace Indie
{
    public class BrainBody : MonoBehaviour
    {
        [SerializeField] private Brain brain;


        // Input
        [SerializeField] private VoiceInput voiceInput;
        [SerializeField] private TMP_InputField inputField;


        private void OnEnable()
        {
            brain?.RegisterScript(GetType(), this);
        }

        private void OnDisable()
        {
            brain?.UnRegisterScript(GetType());
        }


        // Voice Input
        public void StartListening()
        {
            voiceInput?.StartRecording();
        }

        public void StopListening()
        {
            voiceInput?.StopRecording();
        }


        // Get context from voice input
        public async void UnderstandSpeech()
        {
            if (brain) brain.context += "\n Voice Input : " + await voiceInput?.SpeechToText();
        }

        // Get context from text input
        public void Read(string input = null)
        {
            if (brain && !inputField.text.IsNullOrEmpty())  brain.context += $"\n Text Input : {inputField.text}. {input}";
        }

        // text/voice response
        public async void Respond()
        {
            Debug.Log(await brain?.CallChatEndpoint(brain?.context));

            //return await brain?.CallChatResponse(context);
        }

        // Do function call
        public async void Act()
        {
            var response = await brain?.CallFunctionEndpoint(brain?.context);
        }

        [Tool("ForgetConversation", "Clear your message history. Forget about the previous conversation.")]
        public void ForgetConversation() => brain?.ClearHistory();


        [Tool("ForgetContext", "Clear your the context of this conversation. Forget about the context.")]
        public void ForgetContext() { if (brain) brain.context = ""; }


        private void Update()
        {
            // Check if the user has stopped recording
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!voiceInput.isRecording)
                    StartListening();
                else
                {
                    StopListening();
                    UnderstandSpeech();
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log(voiceInput.GetMicrophoneDevices());
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                FastAPICommunicator.CallTest();
            }
        }
    }
}