using UnityEngine;


namespace Indie.Demo
{

    [RequireComponent(typeof(AudioSource))]
    public class SoundFX : MonoBehaviour
    {
        private AudioSource audioSource;

        [SerializeField] private BrainBody brainBody;
        [SerializeField] private CharacterController characterController;

        [Space(10)]
        [SerializeField] private AudioClip pickUpFX;
        [SerializeField] private AudioClip dropFX;
        [SerializeField] private AudioClip thinkFX;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            if (characterController)
            {
                characterController.OnPickUp += PlayPickUpFX;
                characterController.OnRelease += PlayDropFX;
            }

            if (brainBody) brainBody.OnThink += PlayThinkFX;
        }

        private void OnDestroy()
        {
            if (characterController)
            {
                characterController.OnPickUp -= PlayPickUpFX;
                characterController.OnRelease -= PlayDropFX;
            }

            if (brainBody) brainBody.OnThink -= PlayThinkFX;
        }



        public void PlayPickUpFX()
        {
            if (!pickUpFX) return;

            audioSource.clip = pickUpFX;
            audioSource.Play();
        }

        public void PlayDropFX()
        {
            if (!dropFX) return;

            audioSource.clip = dropFX;
            audioSource.Play();
        }

        public void PlayThinkFX()
        {
            if (!thinkFX) return;

            audioSource.clip = thinkFX;
            audioSource.Play();
        }

    }
}
