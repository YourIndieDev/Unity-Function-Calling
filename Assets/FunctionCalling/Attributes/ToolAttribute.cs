using System;

namespace Indie.Attributes
{
    /// <summary>
    /// Attribute used to define a tool, providing its name and description.
    /// </summary>
    public class ToolAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the tool.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolAttribute"/> class with the specified name and description.
        /// </summary>
        /// <param name="name">The name of the tool.</param>
        /// <param name="description">The description of the tool.</param>
        public ToolAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}