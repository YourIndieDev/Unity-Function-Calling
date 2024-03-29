using UnityEngine;
using Indie.OpenAI.Brain;
using Indie.Attributes;
using Indie.Voice;
using TMPro;
using System.Threading.Tasks;


namespace Indie
{
    public class BrainBody : MonoBehaviour
    {
        [Space(10)]
        [Header("Brain")]
        [SerializeField] private Brain brain;

        [Space(10)]
        [Header("Voice")]
        [SerializeField] private VoiceInput voiceInput;
        [SerializeField] private VoiceOutput voiceOutput;

        [Space(10)]
        [Header("Text")]
        [SerializeField] private TMP_InputField textInputField;
        [SerializeField] private TMP_Text textOutputField;


        // Regerister and Unregister this scripts's tools to the brain
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
        public void ReadText(string input = null)
        {
            if (textInputField.text == null) return;
            if (brain && textInputField.text != "")  brain.context += $"\n Text Input : {textInputField.text}. {input}";
        }

        // Get text response
        public async Task<string> TextResponse(string content = null, MessageType messageType = MessageType.user)
        {
            if (content != null)
                return await brain?.CallChatEndpoint(content, messageType);
            else
                return await brain?.CallChatEndpoint(brain?.context, messageType);
        }

        // Get a voice response
        public async Task VoiceResponse(string content = null)
        {
            if (content == null)
                await voiceOutput?.TextToSpeech(await TextResponse());
            else
                await voiceOutput?.TextToSpeech(content);

            voiceOutput?.PlayResponse();
        }

        // Do function call
        public async Task<string> Act()
        {
            return await brain?.CallFunctionEndpoint(brain?.context);
        }


        // Chat with actions (The function that ties it all together)
        public async void ActionChat()
        {
            if (!brain) return;

            // Get the context from the voice input
            await UnderstandSpeech();

            // Get the context from the text input
            ReadText();

            // Get which action to perform
            var actions = await Act();

            // Get a response
            var response = await TextResponse(actions, MessageType.assistant);

            // Display the response
            textOutputField.text = response;

            // Respond with voice
            await VoiceResponse(response);
        }


        // Tools
        [Tool("ForgetConversation", "Clear your message history. Forget about the previous conversation.")]
        public void ForgetConversation() => brain?.ClearHistory();

        [Tool("ForgetContext", "Clear your context of this conversation. Forget about the context.")]
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