using UnityEngine;
using Indie.Attributes;
using Indie.OpenAI.Brain;
using static Indie.OpenAI.Tools.ToolCreator;

namespace Indie.Demo.Examples
{
    public class LightController : MonoBehaviour
    {
        public Brain brain;
        public Light targetLight;

        private void OnEnable()
        {
            // Register this script to the brain
            brain?.RegisterScript(GetType(), this);
        }

        private void OnDisable()
        {
            // Unregister this script from the brain
            brain?.UnRegisterScript(GetType());
        }

        [Tool("TurnOnLight", "Turns on the light.")]
        public void TurnOnLight()
        {
            targetLight.enabled = true; // Turn on the light
        }
    }
}
