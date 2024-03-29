using Newtonsoft.Json;
using System.Collections.Generic;


namespace Indie.OpenAI.Tools
{
    /// <summary>
    /// Utility class for creating tool objects.
    /// </summary>
    public static class ToolCreator
    {
        /// <summary>
        /// Gets or sets the type of the tool.
        /// </summary>
        public class Tool
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("function")]
            public FunctionData Function { get; set; } = new FunctionData();
        }

        /// <summary>
        /// Represents the data of a function.
        /// </summary>
        public class FunctionData
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("parameters")]
            public FunctionParameters Parameters { get; set; } = new FunctionParameters();
        }

        /// <summary>
        /// Represents the parameters of a function.
        /// </summary>
        public class FunctionParameters
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("properties")]
            public Dictionary<string, ParameterDefinition> Properties { get; set; } = new Dictionary<string, ParameterDefinition>();

            [JsonProperty("required")]
            public List<string> Required { get; set; } = new List<string>();
        }

        /// <summary>
        /// Represents the definition of a parameter.
        /// </summary>
        public class ParameterDefinition
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; } = "";

            [JsonProperty("enum")]
            public List<string>? Enum { get; set; } = new List<string>();
        }

        /// <summary>
        /// Creates a tool object with the specified name, description, parameters, and required parameters.
        /// </summary>
        /// <param name="name">The name of the tool.</param>
        /// <param name="description">The description of the tool.</param>
        /// <param name="parameters">Optional. The parameters of the tool.</param>
        /// <param name="requiredParameters">Optional. The required parameters of the tool.</param>
        /// <returns>A new tool object.</returns>
        public static Tool CreateTool(string name, string description, Dictionary<string, ParameterDefinition> parameters = null, List<string> requiredParameters = null)
        {
            var tool = new Tool
            {
                Type = "function",
                Function = new FunctionData
                {
                    Name = name,
                    Description = description,
                    Parameters = new FunctionParameters
                    {
                        Type = "object",
                        Properties = parameters,
                        Required = requiredParameters
                    }
                }
            };

            return tool;
        }
    }
}
