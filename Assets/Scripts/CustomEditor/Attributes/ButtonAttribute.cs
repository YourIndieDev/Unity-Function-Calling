using System;
using UnityEngine;

namespace Danejw
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ButtonAttribute : System.Attribute
    {
        public string ButtonName { get; private set; }

        public ButtonAttribute(string buttonName)
        {
            ButtonName = buttonName;
        }
    }
}
