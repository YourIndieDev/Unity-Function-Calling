using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Indie;


namespace Indie.IEditor
{
    [CustomEditor(typeof(BrainBody))]
    public class BrainBodyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BrainBody brainBody = (BrainBody)target;

            // Get all methods of the BrainBody class
            MethodInfo[] methods = typeof(BrainBody).GetMethods(BindingFlags.Instance | BindingFlags.Public);

            // Iterate through each method
            foreach (MethodInfo method in methods)
            {
                // Check if the method is not a special method (e.g., constructors, operators)
                if (!method.IsSpecialName)
                {
                    // Display a button for the method
                    if (GUILayout.Button(method.Name))
                    {
                        // Invoke the method
                        method.Invoke(brainBody, null);
                    }
                }
            }
        }
    }
}
