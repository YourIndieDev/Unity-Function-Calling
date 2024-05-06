using System;
using System.Collections.Generic;

namespace Indie.Attributes
{
    /// <summary>
    /// Attribute used to define a parameter for a method, providing its name, description, and optional list of enums.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
    public class ParameterAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the description of the parameter.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the list of enum options for the parameter (optional).
        /// </summary>
        public List<string> Enums { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterAttribute"/> class with the specified name, description, and optional list of enums.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="description">The description of the parameter.</param>
        /// <param name="enums">Optional list of enum options for the parameter.</param>
        public ParameterAttribute(string name, string description, params string[] enums)
        {
            Name = name;
            Description = description;
            Enums = new List<string>(enums);
        }
    }
}
