using Doozy.Runtime.Common.Extensions;
using Indie.OpenAI.API;
using Indie.OpenAI.Models.Requests;
using Indie.OpenAI.Models.Responses;
using Indie.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;


namespace Indie.Voice
{
    [RequireComponent(typeof(AudioSource))]
    public class VoiceInput : MonoBehaviour
    {
        public string filename = "recordedClip.wav";
        public AudioSource audioSource;

        // Adjust this value as needed for your project
        public float recordingDuration = 5f;

        public AudioClip recordedClip;

        public bool isRecording = false;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            // Check microphone availability and request permission
            if (Microphone.devices.Length == 0)
            {
                Debug.LogError("No microphone detected!");
                return;
            }

            // Request permission to use the microphone
            if (Application.HasUserAuthorization(UserAuthorization.Microphone) == false)
            {
                Debug.Log("Requesting microphone permission...");
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
            }
        }


        public string GetMicrophoneDevices()
        {
            // Check microphone availability and request permission
            if (Microphone.devices.Length == 0)
            {
                return ("No microphone detected!");
            }

            string devices = "";
            // Print out the available microphone devices
            devices = ("Available microphone devices : ");
            foreach (string device in Microphone.devices)
            {
                devices += (device + " \n");
            }

            return devices;
        }

        // Function to start recording
        public void StartRecording()
        {
            if (!isRecording)
            {
                Debug.Log("Start recording...");

                // Start recording with the default microphone for the specified duration
                recordedClip = Microphone.Start(null, false, Mathf.CeilToInt(recordingDuration), 44100);

                isRecording = true;
            }
            else
            {
                Debug.Log("Already recording.");
            }
        }

        // Function to stop recording and return the recorded AudioClip
        public AudioClip StopRecording()
        {
            if (isRecording)
            {
                Debug.Log("Stop recording.");

                // Stop recording and return the recorded AudioClip
                Microphone.End(null);

                WavUtilities.SaveWavFile(recordedClip, filename);

                isRecording = false;
                return recordedClip;
            }
            else
            {
                Debug.Log("Not recording.");
                return null;
            }
        }

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

    }
}