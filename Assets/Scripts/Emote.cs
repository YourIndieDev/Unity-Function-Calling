using TMPro;
using UnityEngine;
using Indie.OpenAI.Brain;
using static Indie.OpenAI.Tools.ToolCreator;
using Indie.Attributes;
using System.Collections.Generic;
using System;


namespace OpenAiI
{
    public class Emote : MonoBehaviour
    {
        public TMP_InputField inputField;

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
        [Tool("DoEmote", "Allow a character to show emotions")]
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


        [Tool("GiveAnswer", "Give an answer only if asked for")]
        public void GiveAnswer(bool isAsked, string answer)
        {
            Debug.Log($"isAsked: {isAsked} + Answer {answer}");
        }


        public void Think()
        {
            aiBrain.CallFunctionEndpoint(inputField.text);
        }
    }
}
