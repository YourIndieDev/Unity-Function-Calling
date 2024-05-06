using Indie.OpenAI.API;
using Indie.OpenAI.Models.Requests;
using Indie.OpenAI.Models.Responses;
using Indie.Utils;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private byte[] audioData;


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

                var path = WavUtilities.SaveWavFile(recordedClip, filename);

                audioData = File.ReadAllBytes(path);

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
        public async Task<string> SpeechToTextWithPath()
        {
            if (!string.IsNullOrEmpty(filename))
            {
                var speechtotextmessage = new SpeechToTextMessagePath { audio_path = Path.Combine(Application.streamingAssetsPath, filename) };
                var speechToTextAsyncResponse = await FastAPICommunicator.CallEndpointPostAsync<SpeechToText.Response>(FastAPICommunicator.speechToTextPathUrl, speechtotextmessage);

                return speechToTextAsyncResponse.Text;
            }
            else
                return "";
        }

        public async Task<string> SpeechToTextWithData()
        {
            string wavFilePath = Path.Combine(Application.streamingAssetsPath, filename);

            if (File.Exists(wavFilePath))
            {
                // Read the contents of the .wav file
                byte[] audioData = File.ReadAllBytes(wavFilePath);

                var speechToTextAsyncResponse = await FastAPICommunicator.CallEndpointPostAsync<SpeechToText.Response>(FastAPICommunicator.speechToTextDataUrl, audioData, "audio/wav");

                return speechToTextAsyncResponse.Text;
            }
            else
            {
                return "";
            }
        }

        private byte[] ConvertFloatArrayToByteArray(float[] floatArray)
        {
            byte[] byteArray = new byte[floatArray.Length * 4];
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;
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