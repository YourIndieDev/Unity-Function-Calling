using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Indie.Demo
{
    [RequireComponent(typeof(AudioSource))]
    public class WalkingFX : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Rigidbody rb;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!rb) return;

            if (rb.velocity.magnitude > 0)
                PlayWalkingSound();
            else
                audioSource.Stop();
        }

        public void PlayWalkingSound()
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}
