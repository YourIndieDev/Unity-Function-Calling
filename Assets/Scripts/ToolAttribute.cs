using System;


namespace Indie.Attributes
{
    public class ToolAttribute : Attribute
    {
        public string FunctionName { get; }
        public string Description { get; }

        public ToolAttribute(string functionName, string description)
        {
            FunctionName = functionName;
            Description = description;
        }
    }
}