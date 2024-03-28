using UnityEngine;
using Indie.OpenAI.Brain;
using Indie.Attributes;
using Indie.Voice;
using TMPro;
using Indie.OpenAI.API;
using System.Threading.Tasks;


namespace Indie
{
    public class BrainBody : MonoBehaviour
    {
        [SerializeField] private Brain brain;


        // Input
        [SerializeField] private VoiceInput voiceInput;
        [SerializeField] private VoiceOutput voiceOutput;
        [SerializeField] private TMP_InputField textInputField;
        [SerializeField] private TMP_Text textOutputField;


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

            if (brain) brain.context = "";
        }

        public void StopListening()
        {
            voiceInput?.StopRecording();
        }


        // Get context from voice input
        public async Task UnderstandSpeech()
        {
            if (brain) brain.context += "Voice Input : " + await voiceInput?.SpeechToText();
        }

        // Get context from text input
        public void Read(string input = null)
        {
            if (textInputField.text == null) return;
            if (brain && textInputField.text != "")  brain.context += $"\n Text Input : {textInputField.text}. {input}";
        }

        // text response
        public async Task<string> Respond(string content = null, MessageType messageType = MessageType.user)
        {
            if (content != null)
                return await brain?.CallChatEndpoint(content, messageType);
            else
                return await brain?.CallChatEndpoint(brain?.context, messageType);
        }

        // voice response
        public async Task VoiceResponse(string content = null)
        {
            if (content == null)
                await voiceOutput?.TextToSpeech(await Respond());
            else
                await voiceOutput?.TextToSpeech(content);

            voiceOutput?.PlayResponse();
        }

        // Do function call
        public async Task<string> Act()
        {
            return await brain?.CallFunctionEndpoint(brain?.context);
        }


        public async void ActionChat()
        {
            if (!brain) return;

            await UnderstandSpeech();
            Read();
            var actions = await Act();
            var response = await Respond(actions, MessageType.assistant);
            textOutputField.text = response;
            await VoiceResponse(response);
        }


        [Tool("ForgetConversation", "Clear your message history. Forget about the previous conversation.")]
        public void ForgetConversation() => brain?.ClearHistory();


        [Tool("ForgetContext", "Clear your the context of this conversation. Forget about the context.")]
        public void ForgetContext() { if (brain) brain.context = ""; }


        private void Update()
        {
            // Check if the user has stopped recording
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!voiceInput.isRecording)
                    StartListening();
                else
                {
                    StopListening();
                    ActionChat();
                }
            }
        }
    }
}