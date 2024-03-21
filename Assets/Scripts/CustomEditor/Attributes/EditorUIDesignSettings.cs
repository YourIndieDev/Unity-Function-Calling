using UnityEngine;

namespace Danejw.Attribute
{
    [CreateAssetMenu(fileName = "DesignSettings", menuName = "Design/Settings", order = 1)]
    public class DesignSettings : ScriptableObject
    {
        public Color headerColor;
        public Color backgroundColor;
        public Color accentColor;
        // Other design-related fields...
    }
}

