using Doozy.Runtime.Common.Extensions;
using Indie.OpenAI.API;
using Indie.OpenAI.Models.Requests;
using Indie.OpenAI.Models.Responses;
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

                SaveWavFile(recordedClip, filename);

                //voiceInput.audioSource.clip = clip;
                //voiceInput.audioSource.Play();

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


        // Functions to save AudioClip to a WAV file
        public void SaveWavFile(AudioClip clip, string filename)
        {
            string directoryPath = Application.streamingAssetsPath;
            string filePath = Path.Combine(directoryPath, filename);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            // Create a new file stream for writing
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Write the WAV header
                WriteWavHeader(fileStream, clip);

                // Convert AudioClip to PCM data
                float[] samples = new float[clip.samples];
                clip.GetData(samples, 0);

                // Convert PCM data to byte array (16-bit, little-endian)
                byte[] byteArray = ConvertToByteArray(samples);

                // Write PCM data to the file stream
                fileStream.Write(byteArray, 0, byteArray.Length);
            }

            // Refresh the AssetDatabase so the file appears in the Unity Editor
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif
        }

        private void WriteWavHeader(FileStream fileStream, AudioClip clip)
        {
            // Calculate total sample count
            int sampleCount = clip.samples * clip.channels;

            // Calculate total file size (including header)
            int fileSize = sampleCount * sizeof(short) + 44; // 44-byte header

            // Write the "RIFF" chunk
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            fileStream.Write(BitConverter.GetBytes(fileSize - 8), 0, 4);
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"), 0, 4);

            // Write the "fmt " subchunk
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, 4);
            fileStream.Write(BitConverter.GetBytes((int)16), 0, 4); // Subchunk1Size
            fileStream.Write(BitConverter.GetBytes((ushort)1), 0, 2); // AudioFormat (PCM)
            fileStream.Write(BitConverter.GetBytes((ushort)clip.channels), 0, 2); // NumChannels
            fileStream.Write(BitConverter.GetBytes(clip.frequency), 0, 4); // SampleRate
            fileStream.Write(BitConverter.GetBytes(clip.frequency * clip.channels * sizeof(short)), 0, 4); // ByteRate
            fileStream.Write(BitConverter.GetBytes((ushort)(clip.channels * sizeof(short))), 0, 2); // BlockAlign
            fileStream.Write(BitConverter.GetBytes((ushort)16), 0, 2); // BitsPerSample

            // Write the "data" subchunk
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
            fileStream.Write(BitConverter.GetBytes(sampleCount * sizeof(short)), 0, 4);
        }

        private byte[] ConvertToByteArray(float[] samples)
        {
            byte[] byteArray = new byte[samples.Length * sizeof(short)];
            for (int i = 0; i < samples.Length; i++)
            {
                short value = (short)(samples[i] * 32767); // Convert float to short (16-bit PCM)
                byteArray[i * 2] = (byte)(value & 0xff);
                byteArray[i * 2 + 1] = (byte)((value >> 8) & 0xff);
            }
            return byteArray;
        }
    }
}