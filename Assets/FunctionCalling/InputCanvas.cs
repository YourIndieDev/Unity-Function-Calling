using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Indie.Demo.UI
{
    public class InputCanvas : MonoBehaviour
    {
        [SerializeField] private BrainBody brainBody;

        public enum CanvasState
        {
            Idle,
            Processing,
        }
        public CanvasState state = CanvasState.Idle;

        [SerializeField] private Button sendButton;
        [SerializeField] private Button micButton;



        [SerializeField] private Color normalColor;
        [SerializeField] private Color processingColor;

        [SerializeField] private bool awaitingResponse => brainBody.awaitingResponse;
        [SerializeField] private bool isRecording => brainBody.voiceInput.isRecording;

        private void Start()
        {
            sendButton.interactable = true;
            micButton.interactable = true;
        }

        private void Update()
        {
            if (awaitingResponse)
            {
                if (state != CanvasState.Processing)
                {
                    state = CanvasState.Processing;

                    // Send button
                    ToggleSendButton(false, "Thinking...");
                }
            }
            else if (!awaitingResponse)
            {
                if (state != CanvasState.Idle)
                {
                    state = CanvasState.Idle;

                    // Send button
                    ToggleSendButton(true, "Send");                
                }
            }

            if (isRecording)
                ToggleSendButton(false, "Listening...");
            else if (!isRecording && !awaitingResponse)
                ToggleSendButton(true, "Send");
        }

        public void Send()
        {
            brainBody.ActionChat();
        }

        private void ToggleSendButton(bool isActive, string message = "Send")
        {
            if (isActive)
            {
                sendButton.interactable = true;
                sendButton.image.color = normalColor;
                sendButton.GetComponentInChildren<TextMeshProUGUI>().text = message;
            }
            else
            {
                sendButton.interactable = false;
                sendButton.image.color = processingColor;
                sendButton.GetComponentInChildren<TextMeshProUGUI>().text = message;
            }
        }

        public void ToggleMic()
        {
            if (isRecording)
            {
                brainBody.StopListening();
                brainBody.ActionChat();

                micButton.interactable = true;
                micButton.image.color = normalColor;
            }
            else
            {
                micButton.interactable = true;
                micButton.image.color = processingColor;

                brainBody.StartListening();
            }
        }
    }
}
