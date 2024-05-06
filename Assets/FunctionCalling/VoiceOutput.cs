using Indie.OpenAI.API;
using Indie.OpenAI.Models.Requests;
using System.Threading.Tasks;
using UnityEngine;
using Indie.Utils;


namespace Indie.Voice
{
    [RequireComponent(typeof(AudioSource))]
    public class VoiceOutput : MonoBehaviour
    {
        public enum Voice
        {
            alloy,
            echo,
            fable,
            onyx,
            nova,
            shimmer
        }

        [Space(10)]
        [Header("Voice")]
        [SerializeField] private Voice voice = Voice.alloy;

        [Space(10)]
        [Header("Clip Name")]
        [SerializeField] private string filename = "responseClip";


        [SerializeField] private AudioSource audioSource;
        private AudioClip responseClip;


        private void Start()
        {
            if (!audioSource) audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Plays the audio response.
        /// </summary>
        public void PlayResponse()
        {
            if (responseClip == null) return;

            // Play the response
            audioSource.clip = responseClip;
            audioSource.Play();
        }

        /// <summary>
        /// Converts the specified text content to speech and prepares it for playback.
        /// </summary>
        /// <param name="content">The text content to convert to speech.</param>
        public async Task TextToSpeech(string content)
        {
            var textToSpeechMessage = new TextToSpeechMessage
            {
                content = content,
                voice = voice.ToString()
            };

            var audioData = await FastAPICommunicator.CallEndpointPostAsyncForBytes(FastAPICommunicator.textToSpeechUrl, textToSpeechMessage);

            responseClip = WavUtilities.ToAudioClip(audioData, filename);
        }
    }
}
