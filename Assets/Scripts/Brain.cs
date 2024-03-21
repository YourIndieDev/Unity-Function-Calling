using Newtonsoft.Json;
using Indie.OpenAI.Models.Requests;
using Indie.OpenAI.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using static Indie.OpenAI.Tools.ToolCreator;
using Indie.OpenAI.API;
using Indie.Attributes;
using System.ComponentModel;


namespace Indie.OpenAI.Brain
{
    [CreateAssetMenu(fileName = "Brain", menuName = "Indie/Brain", order = 1)]
    public class Brain : ScriptableObject
    {
        [SerializeField] private bool debug = false;

        // The history of the conversation with the AI
        private ChatHistory history = new ChatHistory();

        // list of tools from the scripts
        private List<Tool> toolset = new List<Tool>();

        // list of scripts that have been registered
        private Dictionary<Type, MonoBehaviour> scripts = new Dictionary<Type, MonoBehaviour>();


        // Register the scripts to be analyzed. Get the tools from the scripts and add them to the toolset
        public void RegisterScript(Type script, MonoBehaviour scriptInstance)
        {
            if (!scripts.ContainsKey(script)) scripts.Add(script, scriptInstance);

            var tools = AnalyzeScript(script);

            foreach (var tool in tools)
                AddTool(tool.Function.Name, tool.Function.Description, tool.Function.Parameters.Properties, tool.Function.Parameters.Required);
        }

        public void UnRegisterScript(Type script)
        {
            var tools = AnalyzeScript(script);

            foreach (var tool in tools)
                RemoveTool(tool.Function.Name);

            if (scripts.ContainsKey(script)) scripts.Remove(script);
        }

        private List<Tool> AnalyzeScript(Type script)
        {
            Thought($"Analyzing Scripts For Tools");

            List<Tool> tools = new List<Tool>();


                MethodInfo[] methods = script.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var toolAttribute = method.GetCustomAttribute<ToolAttribute>();
                    if (toolAttribute != null)
                    {
                        List<ParameterInfo> parameters = method.GetParameters().ToList();

                        var parameterDictionary = parameters.ToDictionary(p => p.Name, p => new ParameterDefinition 
                        { 
                            Type = p.ParameterType.Name.ToLower(), 
                            Description = p.Name.ToLower()
                        });

                        List<string> requiredParamters = new List<string>();
                        foreach (var parameter in parameters)
                        {
                            if (!parameter.HasDefaultValue || Nullable.GetUnderlyingType(parameter.ParameterType) == null)
                                requiredParamters.Add(parameter.Name);
                        }

                        var newTool = CreateTool(toolAttribute.FunctionName, toolAttribute.Description, parameterDictionary, requiredParamters);
                        tools.Add(newTool);
                    }
                }

            return tools;
        }


        // Toolset
        private void AddTool(string functionName, string description, Dictionary<string, ParameterDefinition> parameters = null, List<string> requiredParameters = null)
        {
            var tool = toolset.FirstOrDefault(t => t.Function.Name == functionName);
            if (tool != null)
            {
                // Tool found
                tool = CreateTool(functionName, description, parameters, requiredParameters);
                Thought($"Tool found and updated: {tool.Function.Name}");
            }
            else
            {
                // Tool not found, so add it
                var newTool = CreateTool(functionName, description, parameters, requiredParameters);
                toolset.Add(newTool);
                Thought($"Tool added: {functionName}");
            }

        }

        private void RemoveTool(string functionName)
        {
            var tool = toolset.FirstOrDefault(t => t.Function.Name == functionName);
            if (tool != null)
            {
                // Tool found
                toolset.Remove(tool);
                Thought($"Tool found and removed: {tool.Function.Name}.");
            }
            else
            {
                // Tool not found
                Thought($"Tool not found: {functionName}.");
            }
        }

        private void ClearToolset()
        {
            toolset.Clear();

            Thought($"Cleared the toolset.");
        }


        // History
        private void AddHumanMessage(string input)
        {
            history.messages.Add(new ChatMessage { role = "user", content = input });
        }

        private void AddAiMessage(string input)
        {
            history.messages.Add(new ChatMessage { role = "assistant", content = input });
        }

        private void AddSystemMessage(string input)
        {
            history.messages.Add(new ChatMessage { role = "system", content = input });
        }

        public void ClearHistory()
        {
            // Remove messages that are not system messages
            history.messages.RemoveAll(message => message.role != "system");
        }


        private List<string> GetAllFunctionNamesFromToolset()
        {
            var functionNames = toolset
                .Where(t => t.Type == "function")
                .Select(t => t.Function.Name)
                .ToList();
            return functionNames;
        }



        // Create the function message
        private ChatHistory CreateFunctionMessage()
        {
            if (toolset.Count == 0)
                return new ChatHistory { messages = history.messages };
            else
                return new FunctionMessage { messages = history.messages, tools = toolset };
        }

        // Call the function endpoing with an input
        public async void CallFunctionEndpoint(string input)
        {
            AddHumanMessage(input);
            var response = await CallAndParseResponse();

            if (response != null) Thought(response);
        }

        // Call the function and parse the response
        private async Task<string> CallAndParseResponse()
        {
            try
            {
                //Thought(JsonConvert.SerializeObject(CreateFunctionMessage(), Formatting.Indented));

                var functionResponse = await FastAPICommunicator.CallEndpointPostAsync<ChatCompletion.Response>(FastAPICommunicator.functionsUrl, CreateFunctionMessage());

                //Thought(JsonConvert.SerializeObject(functionResponse, Formatting.Indented));

                if (functionResponse.Choices[0].Message.Content != null) AddAiMessage(functionResponse.Choices[0].Message.Content.ToString());

                // Parse function call responses
                foreach (var choice in functionResponse.Choices)
                {
                    if (choice.Message.Content != null) Thought(choice.Message.Content);                    

                    //if (choice.Message.FunctionCall != null) { }

                    if (choice.Message.ToolCalls != null)
                    {
                        foreach (var toolCall in choice.Message.ToolCalls)
                        {
                            Thought($"I should call {toolCall.Function.Name}, with {toolCall.Function.Arguments}.");

                            if (toolCall.Function.Arguments != null) AddAiMessage(toolCall.Function.Arguments);

                            // Deserialize the arguments
                            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(toolCall.Function.Arguments);

                            // Call the function
                            foreach (var script in scripts)
                            {
                                // Get all methods with the ToolAttribute
                                var methods = script.Key.GetMethods()
                                                        .Where(method => Attribute.IsDefined(method, typeof(ToolAttribute)))
                                                        .ToList();

                                foreach (var method in methods)
                                {
                                    Thought($"Checking method: {method.Name}");

                                    // Get the ToolAttribute for the method
                                    var toolAttribute = (ToolAttribute)Attribute.GetCustomAttribute(method, typeof(ToolAttribute));

                                    // Check if the function name matches
                                    if (toolAttribute.FunctionName == toolCall.Function.Name)
                                    {
                                        // Get method parameters
                                        var parameters = method.GetParameters();

                                        // Deserialize arguments from JSON
                                        var argumentDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(toolCall.Function.Arguments);

                                        // Create an array to hold method arguments
                                        var methodArguments = new object[parameters.Length];


                                        // Iterate over each parameter in the method
                                        foreach (var parameter in parameters)
                                        {
                                            // Convert the parameter name to lowercase for case insensitivity
                                            var parameterNameLowercase = parameter.Name.ToLower();

                                            // Find the corresponding argument in the dictionary (case insensitive)
                                            var argument = argumentDictionary.FirstOrDefault(kv => kv.Key.ToLower() == parameterNameLowercase);

                                            // Check if an argument with the same name (case insensitive) exists
                                            if (argument.Key != null)
                                            {
                                                // Convert the value to string
                                                string stringValue = argument.Value.ToString();

                                                // Convert string value to the appropriate parameter type
                                                if (parameter.ParameterType == typeof(bool))
                                                {
                                                    // Handle boolean parameters
                                                    bool boolValue;
                                                    if (!bool.TryParse(stringValue, out boolValue))
                                                        throw new ArgumentException($"Invalid boolean value for parameter '{parameter.Name}' in function '{method.Name}'");
                                                    
                                                    methodArguments[parameter.Position] = boolValue;
                                                }
                                                else
                                                {
                                                    // Convert other parameter types
                                                    var converter = TypeDescriptor.GetConverter(parameter.ParameterType);
                                                    if (converter.CanConvertFrom(typeof(string)))
                                                        methodArguments[parameter.Position] = converter.ConvertFrom(stringValue);
                                                    else
                                                        throw new ArgumentException($"Unsupported parameter type for parameter '{parameter.Name}' in function '{method.Name}'");
                                                }
                                            }
                                            else
                                            {
                                                // Handle missing arguments
                                                throw new ArgumentException($"Missing argument for parameter '{parameter.Name}' in function '{method.Name}'");
                                            }
                                        }

                                        // Invoke the method with the arguments
                                        method.Invoke(script.Value, methodArguments);

                                        // Break out of the inner loop since we found the matching method
                                        break;
                                    }
                                }
                            }
                            return choice.Message.Content;
                        }
                    }
                }
                return default;
            }
            catch (Exception ex)
            {
                Thought($"{ex.Message}", true);
                return default;
            }
        }

        // Logging
        private void Thought(string thought, bool isError = false)
        {
            string prefix = isError ? "<color=red>Contemplation</color>" : "<color=yellow>Thought</color>";
            string formattedLog = $"{prefix} : {thought}";
            if (debug)
            {
                if (isError)
                {
                    Debug.LogError(formattedLog);
                }
                else
                {
                    Debug.Log(formattedLog);
                }
            }
        }
    }
}
