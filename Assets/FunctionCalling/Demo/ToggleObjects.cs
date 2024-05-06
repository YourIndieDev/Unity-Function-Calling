using Indie.Attributes;
using Indie.OpenAI.Brain;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Indie.OpenAI.Tools.ToolCreator;


namespace Indie
{
    public class ToggleObjects : MonoBehaviour
    {
        public Brain aiBrain;
        public TMP_InputField inputField;

        public List<GameObject> objects = new List<GameObject>();

        private void OnEnable()
        {
            aiBrain.RegisterScript(GetType(), this);
        }

        private void OnDisable()
        {
            aiBrain.UnRegisterScript(GetType());
        }

        [Tool("ToggleObject", "Choose the desired object to be displayed")]
        [Parameter("name", "the object to be toggled on or off", "cube", "cylinder", "sphere", "capsule")]
        public void Toggle(string name)
        {
            foreach (var obj in objects)
            {
                if (string.Equals(obj.name, name, StringComparison.OrdinalIgnoreCase))
                    obj.SetActive(true);
                else
                    obj.SetActive(false);
            }
        }
    }
}
