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
    /// <summary>
    /// Enum with the types of messages that can be sent to the AI.
    /// </summary>
    public enum MessageType
    {
        system,
        assistant,
        user
    }

    /// <summary>
    /// Class responsible for managing AI-related functionalities.
    /// </summary>
    [CreateAssetMenu(fileName = "Brain", menuName = "Indie/Brain", order = 1)]
    public class Brain : ScriptableObject
    {
        /// <summary>
        /// Flag indicating whether to show thoughts.
        /// </summary>
        [SerializeField] private bool showThoughts = false;

        /// <summary>
        /// Flag indicating whether to show actions.
        /// </summary>
        [SerializeField] private bool showActions = false;

        [Space(20)]


        // Input

        /// <summary>
        /// A system message that the AI will use as guidance througout the interaction.
        /// </summary>
        [SerializeField, TextArea(3, 50)]
        private string systemMessage = "";

        [Space(20)]

        /// <summary>
        /// The history of the conversation with the AI.
        /// </summary>
        [SerializeField] private ChatHistory history = new ChatHistory();

        [Space(20)]


        // Output

        /// <summary>
        /// Output context.
        /// </summary>
        [SerializeField, TextArea(3, 50)]
        public string context;

        /// <summary>
        /// List of tools from the scripts.
        /// </summary>
        private List<Tool> toolset = new List<Tool>();

        /// <summary>
        /// Dictionary of scripts that have been registered.
        /// </summary>
        private Dictionary<Type, MonoBehaviour> scripts = new Dictionary<Type, MonoBehaviour>();


        // Analyze Scripts

        /// <summary>
        /// Registers a script to be analyzed for tools.
        /// </summary>
        /// <param name="script">The type of the script to register.</param>
        /// <param name="scriptInstance">The instance of the script.</param>
        public void RegisterScript(Type script, MonoBehaviour scriptInstance)
        {
            if (!scripts.ContainsKey(script)) scripts.Add(script, scriptInstance);

            var tools = AnalyzeScript(script);

            foreach (var tool in tools)
                AddTool(tool.Function.Name, tool.Function.Description, tool.Function.Parameters.Properties, tool.Function.Parameters.Required);
        }

        /// <summary>
        /// Unregisters a script.
        /// </summary>
        /// <param name="script">The type of the script to unregister.</param>
        public void UnRegisterScript(Type script)
        {
            var tools = AnalyzeScript(script);

            foreach (var tool in tools)
                RemoveTool(tool.Function.Name);

            if (scripts.ContainsKey(script)) scripts.Remove(script);
        }

        /// <summary>
        /// Analyzes a script to extract tools.
        /// </summary>
        /// <param name="script">The type of the script to analyze.</param>
        /// <returns>A list of tools extracted from the script.</returns>
        private List<Tool> AnalyzeScript(Type script)
        {
            Thought($"Analyzing Scripts For Tools");

            List<Tool> tools = new List<Tool>();


            MethodInfo[] methods = script.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods)
            {
                // Checks the rcript for the ToolAttribute
                var toolAttribute = method.GetCustomAttribute<ToolAttribute>();

                // Get all ParameterAttributes
                var parameterAttributes = method.GetCustomAttributes<ParameterAttribute>();

                // Setup the parameter dictionary and required parameters list
                var parameterDictionary = new Dictionary<string, ParameterDefinition>();
                List<string> requiredParameters = new List<string>();

                if (toolAttribute != null)
                {
                    // If there is no parameter attribute, contruct them with the information we have
                    if (parameterAttributes.Count() == 0)
                    {
                        // Get the parameters of the method
                        List<ParameterInfo> parameters = method.GetParameters().ToList();

                        if (parameters.Count > 0)
                        {
                            // Setup the parameters if the attributes are present in the parameter level
                            foreach (var parameter in parameters)
                            {
                                var paramAttr = parameter.GetCustomAttribute<ParameterAttribute>();
                                string paramDescription = paramAttr != null ? paramAttr.Description : parameter.Name.ToLower();
                                List<string> enumOptions = paramAttr?.Enums;

                                parameterDictionary.Add(parameter.Name, new ParameterDefinition
                                {
                                    Type = parameter.ParameterType.Name.ToLower(),
                                    Description = paramDescription,
                                    Enum = enumOptions
                                });

                                // Create the list of required parameters from the parameter info
                                if (!parameter.HasDefaultValue || Nullable.GetUnderlyingType(parameter.ParameterType) == null)
                                    requiredParameters.Add(parameter.Name);
                            }
                        }
                    }
                    // If the parameter attributes are present on the method level, use them to construct the parameters
                    else
                    {
                        // Setup the parameters if the attributes are present in the parameter level
                        foreach (var paramAttr in parameterAttributes)
                        {
                            // Get the paramter of the method with the same name
                            var parameter = method.GetParameters().FirstOrDefault(p => p.Name.ToLower() == paramAttr.Name.ToLower());

                            List<string> enumOptions = paramAttr?.Enums;

                            parameterDictionary.Add(paramAttr.Name, new ParameterDefinition
                            {
                                Type = parameter.ParameterType.Name.ToLower(),
                                Description = paramAttr.Description,
                                Enum = enumOptions
                            });

                            // Create the list of required parameters from the parameter info
                            if (!parameter.HasDefaultValue || Nullable.GetUnderlyingType(parameter.ParameterType) == null)
                                requiredParameters.Add(parameter.Name);
                        }
                    }
                                     
                    var newTool = CreateTool(toolAttribute.Name, toolAttribute.Description, parameterDictionary, requiredParameters);
                    tools.Add(newTool);

                    Thought($"Tool analyzed and added: {JsonConvert.SerializeObject(newTool, Formatting.Indented)}");
                }
            }

            return tools;
        }


        // Toolset

        /// <summary>
        /// Adds or updates a tool in the toolset.
        /// </summary>
        /// <param name="functionName">The name of the function associated with the tool.</param>
        /// <param name="description">The description of the tool.</param>
        /// <param name="parameters">Optional. The parameters of the tool.</param>
        /// <param name="requiredParameters">Optional. The required parameters of the tool.</param>
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

        /// <summary>
        /// Removes a tool from the toolset.
        /// </summary>
        /// <param name="functionName">The name of the function associated with the tool.</param>
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

        /// <summary>
        /// Clears the toolset.
        /// </summary>
        private void ClearToolset()
        {
            toolset.Clear();

            Thought($"Cleared the toolset.");
        }


        // History

        /// <summary>
        /// Adds a human message to the chat history.
        /// </summary>
        /// <param name="input">The message content.</param>
        /// <returns>The chat message object added to the history.</returns>
        private ChatMessage AddHumanMessage(string input)
        {
            var chatmessage = new ChatMessage { role = "user", content = input };
            history.messages.Add(chatmessage);
            return chatmessage;
        }

        /// <summary>
        /// Adds an AI message to the chat history.
        /// </summary>
        /// <param name="input">The message content.</param>
        private void AddAIMessage(string input)
        {
            history.messages.Add(new ChatMessage { role = "assistant", content = input });
        }

        /// <summary>
        /// Adds or updates a system message in the chat history.
        /// </summary>
        /// <param name="input">The message content.</param>
        private void AddSystemMessage(string input)
        {
            // Check if a system message already exists
            ChatMessage systemMessage = history.messages.Find(message => message.role == "system");

            // Update the existing system message's content or add it if it doesn't exist
            if (systemMessage != null) systemMessage.content = input;
            else history.messages.Add(new ChatMessage { role = "system", content = input });
        }

        /// <summary>
        /// Clears non-system messages from the chat history.
        /// </summary>
        public void ClearHistory()
        {
            // Remove messages that are not system messages
            history.messages.RemoveAll(message => message.role != "system");
        }

        /// <summary>
        /// Retrieves all function names from the toolset.
        /// </summary>
        /// <returns>A list of function names.</returns>
        private List<string> GetAllFunctionNamesFromToolset()
        {
            var functionNames = toolset
                .Where(t => t.Type == "function")
                .Select(t => t.Function.Name)
                .ToList();
            return functionNames;
        }


        // Calls

        /// <summary>
        /// Calls the chat endpoint with the provided input message.
        /// </summary>
        /// <param name="input">The input message.</param>
        /// <param name="messageType">The type of message (user, system, or assistant).</param>
        /// <returns>The response from the chat endpoint.</returns>
        public async Task<string> CallChatEndpoint(string input, MessageType messageType = MessageType.user)
        {
            switch (messageType)
            {
                case MessageType.system:
                    AddSystemMessage(input);
                    break;
                case MessageType.assistant:
                    AddAIMessage(input);
                    break;
                case MessageType.user:
                    AddHumanMessage(input);
                    break;
            }

            var chatAsyncResponse = await FastAPICommunicator.CallEndpointPostAsync<ChatCompletion.Response>(FastAPICommunicator.chatHistoryAsyncUrl, history);

            AddAIMessage(chatAsyncResponse.Choices[0].Message.Content);
            return chatAsyncResponse.Choices[0].Message.Content;
        }

        /// <summary>
        /// Creates a function message for calling function endpoints.
        /// </summary>
        /// <returns>The function message containing chat history and tools.</returns>
        private ChatHistory CreateFunctionMessage()
        {
            // update the system message
            AddSystemMessage(systemMessage);

            if (toolset.Count == 0)
                return new ChatHistory { messages = history.messages };
            else
                return new FunctionMessage { messages = history.messages, tools = toolset };
        }

        /// <summary>
        /// Calls a function endpoint with the provided input.
        /// </summary>
        /// <param name="input">The input for the function.</param>
        /// <returns>The response from the function endpoint.</returns>
        public async Task<string> CallFunctionEndpoint(string input)
        {
            AddHumanMessage(input);
            var response = await CallAndParseResponse();

            if (response != null)
                return response;
            else
                return "";
        }

        /// <summary>
        /// Calls a function endpoint and parses the response.
        /// </summary>
        /// <returns>The parsed response from the function endpoint.</returns>
        private async Task<string> CallAndParseResponse()
        {
            try
            {
                var functionResponse = await FastAPICommunicator.CallEndpointPostAsync<ChatCompletion.Response>(FastAPICommunicator.functionsUrl, CreateFunctionMessage());

                if (functionResponse.Choices[0].Message.Content != null) AddAIMessage(functionResponse.Choices[0].Message.Content.ToString());

                // Parse function call responses
                foreach (var choice in functionResponse.Choices)
                {
                    if (choice.Message.Content != null) Thought(choice.Message.Content);

                    //if (choice.Message.FunctionCall != null) { }

                    string actions = "";
                    if (choice.Message.ToolCalls != null)
                    {
                        foreach (var toolCall in choice.Message.ToolCalls)
                        {
                            Thought($"I should call {toolCall.Function.Name}, with {toolCall.Function.Arguments}.", isAction: true);

                            if (toolCall.Function.Arguments != null)
                            {
                                actions += $"{toolCall.Function.Name}, {toolCall.Function.Arguments} \n";
                                //AddAiMessage($"I am calling {toolCall.Function.Name}, with {toolCall.Function.Arguments}.");
                            }

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
                                    if (toolAttribute.Name == toolCall.Function.Name)
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
                        }
                    }
                    return $"{actions}"; // choice.Message.Content;
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

        /// <summary>
        /// Logs a thought, contemplation, or action.
        /// </summary>
        /// <param name="thought">The thought or message to log.</param>
        /// <param name="isError">Whether the thought is an error.</param>
        /// <param name="isAction">Whether the thought is an action.</param>
        private void Thought(string thought, bool isError = false, bool isAction = false)
        {
            string prefix = isError ? "<color=red>Contemplation</color>" : (isAction ? "<color=orange>Action</color>" : "<color=yellow>Thought</color>");
            string formattedLog = $"{prefix} : {thought}";

            if (showThoughts)
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
            else if (showActions && isAction)
            {
                Debug.Log(formattedLog);
            }
        }
    }
}
