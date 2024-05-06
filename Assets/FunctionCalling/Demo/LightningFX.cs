using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Indie.Demo
{
    [RequireComponent(typeof(AudioSource))]
    public class LightningFX : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> lightningClips;

        public float minDelay;
        public float maxDelay;


        private AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            // Start the coroutine to play random audio clips
            StartCoroutine(PlayRandomAudio());
        }

        IEnumerator PlayRandomAudio()
        {
            while (true)
            {
                // Wait for a random delay before playing the next audio clip
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);

                // Check if there are audio clips available
                if (lightningClips != null && lightningClips.Count > 0)
                {
                    // Play a random audio clip
                    AudioClip randomClip = lightningClips[Random.Range(0, lightningClips.Count)];
                    audioSource.clip = randomClip;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogWarning("No audio clips assigned.");
                }
            }
        }


    }
}
