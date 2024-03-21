using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Linq;

namespace Danejw.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var mono = target as MonoBehaviour;

            // Get all methods with the 'Button' attribute
            var methods = mono.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => System.Attribute.IsDefined(m, typeof(ButtonAttribute)));

            foreach (var methodInfo in methods)
            {
                // Get the ButtonAttribute on the method
                ButtonAttribute attr = (ButtonAttribute)System.Attribute.GetCustomAttribute(methodInfo, typeof(ButtonAttribute));

                if (GUILayout.Button(attr.ButtonName))
                {
                    // If the button is clicked, invoke the method
                    methodInfo.Invoke(mono, null);
                }
            }
        }
    }
}
