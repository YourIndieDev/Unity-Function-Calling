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
        public Voice voice = Voice.alloy;

        [SerializeField] private string filename = "responseClip";
        [SerializeField] private AudioSource audioSource;
        private AudioClip responseClip;


        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayResponse()
        {
            // Play the response
            audioSource.clip = responseClip;
            audioSource.Play();
        }

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
