using UnityEngine;
using Indie.Attributes;
using Indie.OpenAI.Brain;
using static Indie.OpenAI.Tools.ToolCreator;

namespace YourNamespace
{
    public class ColorChanger : MonoBehaviour
    {
        public Brain brain;
        public Renderer objectRenderer;

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

        [Tool("ChangeColor", "Changes the color of a material.")]
        [Parameter("material", "The material whose color should be changed")]
        [Parameter("color", "The new color for the material")]
        public void ChangeColor(Material material, Color color)
        {
            objectRenderer.sharedMaterial = material;
            objectRenderer.material.color = color;
        }
    }
}