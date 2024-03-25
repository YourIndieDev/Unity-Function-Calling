using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Indie.OpenAI.Tools
{
    public static class ToolCreator
    {
        // Chat Functions/Tools Models
        public class Tool
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("function")]
            public FunctionData Function { get; set; } = new FunctionData();
        }

        public class FunctionData
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("parameters")]
            public FunctionParameters Parameters { get; set; } = new FunctionParameters();
        }

        public class FunctionParameters
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("properties")]
            public Dictionary<string, ParameterDefinition> Properties { get; set; } = new Dictionary<string, ParameterDefinition>();

            [JsonProperty("required")]
            public List<string> Required { get; set; } = new List<string>();
        }

        public class ParameterDefinition
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; } = "";

            [JsonProperty("enum")]
            public List<string>? Enum { get; set; } = new List<string>();
        }

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

        public static string CreateToolJson(List<Tool> tools)
        {
            return JsonConvert.SerializeObject(new { tools });
        }


        /*
        var toolBuilder = new ToolBuilder()
            .WithName(name)
            .WithDescription(description);

        foreach (var parameter in parameters)
            toolBuilder.WithParameter(parameter.Key, parameter.Value.type, parameter.Value.description, parameter.Value.enumOptions);

        return toolBuilder.BuildJson();
        */
    }
    



    public class ToolBuilder
    {
        private string _name;
        private string _description;
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public ToolBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ToolBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ToolBuilder WithParameter(string name, string type, string description, List<string> enumOptions = null)
        {
            var parameter = new Dictionary<string, object>
            {
                { "type", type },
                { "description", description }
            };

            if (enumOptions != null)
            {
                parameter.Add("enum", enumOptions);
            }

            _parameters[name] = parameter;
            return this;
        }

        public string BuildJson()
        {
            if (string.IsNullOrEmpty(_name))
                throw new ArgumentException("Name cannot be empty");

            if (string.IsNullOrEmpty(_description))
                throw new ArgumentException("Description cannot be empty");

            if (_parameters.Count == 0)
                throw new ArgumentException("At least one parameter must be provided");

            var tool = new
            {
                type = "function",
                function = new
                {
                    name = _name,
                    description = _description,
                    parameters = new
                    {
                        type = "object",
                        properties = _parameters,
                        required = _parameters.Keys
                    }
                }
            };

            return JsonConvert.SerializeObject(tool);
        }
    }
}
