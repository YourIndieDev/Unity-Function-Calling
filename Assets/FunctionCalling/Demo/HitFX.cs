using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Indie.Demo
{
    [RequireComponent(typeof(AudioSource))]
    public class HitFX : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> hitFXs;

        private void Start()
        {
            if (!audioSource) audioSource = GetComponent<AudioSource>();     
        }

        private void OnCollisionEnter(Collision collision)
        {
            PlayHitFX();
        }

        private void PlayHitFX()
        {
            // Play a random hit sound
            if (hitFXs.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, hitFXs.Count);
                audioSource.clip = hitFXs[randomIndex];
                audioSource.Play();
            }
        }
    }
}
