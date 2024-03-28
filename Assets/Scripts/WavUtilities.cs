using System;
using System.IO;
using UnityEngine;


namespace Indie.Utils
{
    public static class WavUtilities
    {
        // Functions to save AudioClip to a WAV file
        public static void SaveWavFile(AudioClip clip, string filename)
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

        private static void WriteWavHeader(FileStream fileStream, AudioClip clip)
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

        private static byte[] ConvertToByteArray(float[] samples)
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

        public static AudioClip ToAudioClip(byte[] wavFile, string clipName)
        {
            //PrintHeaderBytes(wavFile);
            //CheckWavMetadata(wavFile);

            // Get the WAV file's header information
            int channels = BitConverter.ToInt16(wavFile, 22);
            int sampleRate = BitConverter.ToInt32(wavFile, 24);
            int byteRate = BitConverter.ToInt32(wavFile, 28);
            int blockAlign = BitConverter.ToInt16(wavFile, 32);
            int bitsPerSample = BitConverter.ToInt16(wavFile, 34);
            int headerOffset = 44;

            float[] audioData = new float[(wavFile.Length - headerOffset) / 2];

            // Convert the byte array to the audio data
            for (int i = 0; i < audioData.Length; i++)
                audioData[i] = BitConverter.ToInt16(wavFile, i * 2 + headerOffset) / 32768f;

            //Debug.Log($"Channels: {channels}, SampleRate: {sampleRate}, ByteRate: {byteRate}, BlockAlign: {blockAlign}, BitsPerSample: {bitsPerSample}");

            // Create the audio clip
            AudioClip audioClip = AudioClip.Create(clipName, audioData.Length, channels, sampleRate, false);
            audioClip.SetData(audioData, 0);

            return audioClip;
        }

        private static void CheckWavMetadata(byte[] wavBytes)
        {
            if (wavBytes.Length < 44)
            {
                Debug.Log("File too short to be a valid WAV file");
                return;
            }

            string riff = System.Text.Encoding.ASCII.GetString(wavBytes, 0, 4);
            string wave = System.Text.Encoding.ASCII.GetString(wavBytes, 8, 4);
            short format = BitConverter.ToInt16(wavBytes, 20);
            int channels = BitConverter.ToInt16(wavBytes, 22);
            int sampleRate = BitConverter.ToInt32(wavBytes, 24);
            int byteRate = BitConverter.ToInt32(wavBytes, 28);
            short blockAlign = BitConverter.ToInt16(wavBytes, 32);
            short bitsPerSample = BitConverter.ToInt16(wavBytes, 34);

            Debug.Log($"RIFF: {riff}, WAVE: {wave}, Format: {format}, Channels: {channels}, SampleRate: {sampleRate}, ByteRate: {byteRate}, BlockAlign: {blockAlign}, BitsPerSample: {bitsPerSample}");
        }

        private static void PrintHeaderBytes(byte[] wavBytes)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("Header Bytes: ");
            for (int i = 0; i < Math.Min(wavBytes.Length, 48); i++) // Print first 48 bytes
            {
                sb.Append(wavBytes[i].ToString("X2") + " ");
            }
            Debug.Log(sb.ToString());
        }
    }

}
