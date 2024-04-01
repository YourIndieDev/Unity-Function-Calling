using UnityEngine;
using Indie.OpenAI.Brain;
using static Indie.OpenAI.Tools.ToolCreator;
using Indie.Attributes;
using System.Collections.Generic;
using System;


namespace Indie.Demo
{
    public class Emote : MonoBehaviour
    {
        public Brain aiBrain;

        public List<GameObject> emoji = new List<GameObject>();

        private void OnEnable()
        {
            aiBrain.RegisterScript(GetType(), this);
        }

        private void OnDisable()
        {
            aiBrain.UnRegisterScript(GetType());
        }


        // Function to call
        [Tool("DoEmote", "Allow a character to show emotions.")]
        [Parameter("emotion", "The emotion to feel", "Happy", "Sad", "Anger", "Suprised", "Anxiety", "Disgust", "Fear", "Gratitude", "Love")]
        public void DoEmote(string emotion)
        {
            foreach (var obj in emoji)
            {
                if (string.Equals(obj.name, emotion, StringComparison.OrdinalIgnoreCase))
                    obj.SetActive(true);
                else
                    obj.SetActive(false);
            }
        }
    }
}
