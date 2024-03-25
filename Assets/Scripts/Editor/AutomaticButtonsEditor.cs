using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Sim
{
    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    public class AutomaticButtonsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Get the target object's type
            var targetType = target.GetType();

            // Get all public instance methods of the target object
            MethodInfo[] methodInfo = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (MethodInfo method in methodInfo)
            {
                // Ensure that we only draw buttons for methods without parameters
                if (method.GetParameters().Length == 0)
                {
                    if (GUILayout.Button(method.Name))
                    {
                        // Invoke the method when the button is clicked
                        method.Invoke(target, null);
                    }
                }
            }
        }
    }
}
