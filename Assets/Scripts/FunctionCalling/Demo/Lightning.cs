using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Indie.Demo
{
    public class Lightning : MonoBehaviour
    {
        public Light directionalLight;
        public AudioSource audioSource;

        public float minIntensity = 0f;
        public float maxIntensity = 5f;
        public float multiplier = 1f;

        void Update()
        {
            // Check if audio source and directional light are assigned
            if (audioSource != null && directionalLight != null)
            {
                // Get the audio spectrum data
                float[] spectrumData = new float[512]; // Adjust the size according to your requirements
                audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

                // Calculate the average amplitude of the samples
                float averageAmplitude = 0f;
                for (int i = 0; i < spectrumData.Length; i++)
                {
                    averageAmplitude += spectrumData[i];
                }
                averageAmplitude /= spectrumData.Length;

                // Map the average amplitude to the intensity of the directional light
                directionalLight.intensity = Remap(averageAmplitude, 0f, 1f, minIntensity, maxIntensity) * multiplier; // Adjust the intensity range as needed
            }
            else
            {
                Debug.LogWarning("Audio source or directional light is not assigned.");
            }
        }

        // Remap function to map a value from one range to another
        float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
