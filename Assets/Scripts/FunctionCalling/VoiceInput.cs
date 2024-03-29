using Doozy.Runtime.Common.Extensions;
using Indie.OpenAI.API;
using Indie.OpenAI.Models.Requests;
using Indie.OpenAI.Models.Responses;
using Indie.Utils;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;


namespace Indie.Voice
{
    [RequireComponent(typeof(AudioSource))]
    public class VoiceInput : MonoBehaviour
    {
        [Space(10)]
        [Header("Debug")]
        [SerializeField] private bool debug = false;

        [Space(10)]
        [Header("The name of the file")]
        public string filename = "recordedClip.wav";

        // Adjust this value as needed for your project
        [Space(10)]
        [Header("Recording Duration")]
        public float recordingDuration = 5f;


        private AudioSource audioSource;
        private AudioClip recordedClip;
        [HideInInspector] public bool isRecording = false;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            // Check microphone availability and request permission
            if (Microphone.devices.Length == 0)
            {
                Log("No microphone detected!", true);
                return;
            }

            // Request permission to use the microphone
            if (Application.HasUserAuthorization(UserAuthorization.Microphone) == false)
            {
                Log("Requesting microphone permission...");
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
            }
        }

        /// <summary>
        /// Gets a list of available microphone devices.
        /// </summary>
        /// <returns>A string containing the names of available microphone devices.</returns>
        public string GetMicrophoneDevices()
        {
            // Check microphone availability and request permission
            if (Microphone.devices.Length == 0)
                return ("No microphone detected!");

            string devices = "";

            // Print out the available microphone devices
            devices = ("Available microphone devices : ");
            foreach (string device in Microphone.devices)
                devices += (device + " \n");

            return devices;
        }

        // <summary>
        /// Starts recording audio from the default microphone.
        /// </summary>
        public void StartRecording()
        {
            if (!isRecording)
            {
                Log("Start recording...");

                // Start recording with the default microphone for the specified duration
                recordedClip = Microphone.Start(null, false, Mathf.CeilToInt(recordingDuration), 44100);

                isRecording = true;
            }
            else
            {
                Log("Already recording.");
            }
        }

        /// <summary>
        /// Stops recording audio and saves the recorded AudioClip as a WAV file.
        /// </summary>
        /// <returns>The recorded AudioClip.</returns>
        public AudioClip StopRecording()
        {
            if (isRecording)
            {
                Log("Stop recording.");

                // Stop recording and return the recorded AudioClip
                Microphone.End(null);

                WavUtilities.SaveWavFile(recordedClip, filename);

                isRecording = false;
                return recordedClip;
            }
            else
            {
                Log("Not recording.");
                return null;
            }
        }

        /// <summary>
        /// Converts the recorded audio to text using a speech-to-text service.
        /// </summary>
        /// <returns>The text transcription of the recorded audio.</returns>
        public async Task<string> SpeechToText()
        {
            if (!filename.IsNullOrEmpty())
            {
                var speechtotextmessage = new SpeechToTextMessage { audio_path = Path.Combine(Application.streamingAssetsPath, filename) };
                var speechToTextAsyncResponse = await FastAPICommunicator.CallEndpointPostAsync<SpeechToText.Response>(FastAPICommunicator.speechToTextUrl, speechtotextmessage);

                return speechToTextAsyncResponse.Text;
            }
            else
                return "";
        }

        /// <summary>
        /// Logs messages to the Unity console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="isError">Specifies whether the message is an error message.</param>
        private void Log(string message, bool isError = false)
        {
            if (!debug) return;

            if (!isError)
                Debug.Log(message);
            else
                Debug.LogError(message);
        }
    }
}