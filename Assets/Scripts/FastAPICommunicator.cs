using Newtonsoft.Json;
using Indie.OpenAI.Models.Requests;
using Indie.OpenAI.Models.Responses;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;



namespace Indie.OpenAI.API 
{
    public static class UnityWebRequestExtensions
{
    public static Task<UnityWebRequest.Result> AsTask(this UnityWebRequestAsyncOperation asyncOp)
    {
        var completionSource = new TaskCompletionSource<UnityWebRequest.Result>();
        asyncOp.completed += _ => completionSource.TrySetResult(asyncOp.webRequest.result);
        return completionSource.Task;
    }
}
    public static class FastAPICommunicator
    {
        public static string chatAsyncUrl = "http://localhost:8000/chat_async/";
        public static string visionUrl = "http://localhost:8000/vision_async_url/";
        public static string visionBytesUrl = "http://localhost:8000/vision_async_bytes/";
        public static string imageAsyncsUrl = "http://localhost:8000/image_async/";
        public static string speechToTextTimeStampUrl = "http://localhost:8000/speech_to_text_timestamp_async/";
        public static string speeachToTextUrl = "http://localhost:8000/speech_to_text_async/";
        public static string moderationsAsyncUrl = "http://localhost:8000/moderations_async/";
        public static string assistantUrl = "http://localhost:8000/assistant/";
        public static string functionsUrl = "http://localhost:8000/chat_functions/";


        static async void Start()
        {
            //var chatmessage = new ChatMessage { input = "Create a random image prompt" };
            //var visionmessage = new VisionMessage { url = "", input = "Explain this" };
            //var visionbytesmessage = new VisionBytesMessage { image_path = "", input = "Explain this" };
            var speechtotextmessage = new SpeechToTextMessage { audio_path = @"C:\\Users\RecallableFacts\Desktop\ElevenLabs_2024-02-26T07_20_33_NarratorDano_ivc_s76_sb75_se50_b_m2.mp3" };
            //var assistantmessage = new AssistantMessage { input = "Create a story of An astronaut exploring a lush, alien jungle filled with towering, glowing mushrooms and mysterious, bioluminescent creatures.", assistant_id = "asst_ZE2w0PwhCO6XWJk7rZB1xAcD"};
            //var imagePrompt = new ChatMessage{ input = "An astronaut exploring a lush, alien jungle filled with towering, glowing mushrooms and mysterious, bioluminescent creatures." };

            /*
                // Create history and input message
                var history = new List<ChatMessage>
                {
                    new ChatMessage { role = "system", content = "Don't make assumptions about what values to plug into functions. Ask for clarification if a user request is ambiguous." },
                    new ChatMessage { role = "user", content = "can you get the dog to get me a beer?" }
                };

                // Example Emotions Tool to Json
                var emotionsOptions = new List<string> 
                {
                    "happy",
                    "sad",
                    "angry",
                    "fearful",
                    "surprised",
                    "disgusted",
                    "content",
                    "excited",
                    "relaxed",
                    "nervous",
                    "hopeful",
                    "disappointed",
                    "jealous",
                    "guilty",
                    "lonely",
                    "proud",
                    "embarrassed",
                    "shy",
                    "insecure",
                    "confident",
                    "loved",
                    "rejected",
                    "bored",
                    "curious",
                    "determined",
                    "frustrated",
                    "overwhelmed",
                    "peaceful",
                    "motivated",
                    "grateful",
                    "resentful",
                    "compassionate",
                    "inspired",
                    "nostalgic",
                    "satisfied",
                    "regretful",
                    "amused",
                    "energetic",
                    "apprehensive",
                    "optimistic",
                    "courageous",
                    "defeated",
                    "hopeless",
                    "indifferent",
                    "withdrawn",
                    "sympathetic",
                    "envious",
                    "elated",
                    "depressed",
                    "agitated",
                    "serene",
                    "suspicious",
                    "compassionate",
                    "charmed",
                    "disappointed",
                    "rejuvenated",
                    "weary",
                    "amazed",
                    "blissful",
                    "startled",
                    "devastated",
                    "touched",
                    "worried",
                    "vulnerable",
                    "fascinated",
                    "restless",
                    "encouraged",
                    "uncomfortable",
                    "melancholic",
                    "exhilarated",
                    "relieved",
                    "terrified",
                    "overjoyed",
                    "thrilled",
                    "helpless",
                    "hurt",
                    "grumpy",
                    "eager",
                    "grief-stricken",
                    "insignificant",
                    "comforted",
                    "powerless",
                    "dissatisfied",
                    "livid",
                    "alarmed",
                    "cheerful",
                    "gloomy",
                    "rebellious",
                    "elated",
                    "forgiving",
                    "flustered",
                    "giddy",
                    "melancholy",
                    "nostalgic",
                    "unhappy",
                    "tranquil",
                    "blissful",
                    "jittery",
                    "animated",
                    "euphoric",
                    "optimistic",
                    "anguished",
                    "festive",
                    "grouchy",
                    "playful",
                    "panicked",
                    "self-conscious",
                    "exhausted",
                    "enraged",
                    "doubtful",
                    "gracious",
                    "motivated",
                    "exhilarated",
                    "humbled",
                    "jubilant",
                    "self-assured",
                    "melancholic",
                    "dejected",
                    "disheartened",
                    "appalled",
                    "ecstatic",
                    "tranquil",
                    "refreshed",
                    "hopeful",
                    "overwhelmed",
                    "pessimistic",
                    "ambivalent",
                    "stressed",
                    "frightened",
                    "rejuvenated",
                    "forlorn",
                    "accepting",
                    "weary",
                    "disillusioned",
                    "shocked",
                    "joyful",
                    "despondent",
                    "mournful",
                    "enlightened",
                    "accomplished",
                    "merry",
                    "gleeful",
                    "apprehensive",
                    "receptive",
                    "thrilled",
                    "dejected",
                    "defensive",
                    "inadequate",
                    "crestfallen",
                    "uncertain",
                    "jovial",
                    "resentful",
                    "irritable",
                    "disappointed",
                    "disheartened",
                    "defiant",
                    "inspired",
                    "upset",
                    "inadequate",
                    "amused",
                    "hopeful",
                    "impatient",
                    "touched",
                    "accomplished",
                    "optimistic",
                    "dismayed",
                    "disenchanted",
                    "enthusiastic",
                    "annoyed",
                    "thankful",
                    "zestful",
                    "aggravated",
                    "sorrowful",
                    "appreciated",
                    "irritated",
                    "grateful",
                    "uncertain",
                    "stressed",
                    "charitable",
                    "pessimistic",
                    "regretful",
                    "amused",
                    "dejected",
                    "hopeless",
                    "overjoyed",
                    "grateful",
                    "agitated",
                    "invincible",
                    "doubtful",
                    "nostalgic",
                    "crushed",
                    "uplifted",
                    "weary",
                    "unworthy",
                    "fulfilled",
                    "disappointed",
                    "insecure",
                    "ecstatic",
                    "rejected",
                    "envious",
                    "fretful",
                    "fulfilled",
                    "heartbroken",
                    "proud",
                    "enthusiastic",
                    "miserable",
                    "disillusioned",
                    "optimistic",
                    "restless",
                    "fearful",
                    "crestfallen",
                    "zealous",
                    "rejected",
                    "insecure",
                    "disgruntled",
                    "fulfilled",
                    "glum",
                    "discouraged",
                    "unimportant",
                    "alive",
                    "depressed",
                    "impatient",
                    "resigned",
                    "unsure",
                    "enlightened",
                    "energetic",
                    "listless",
                    "furious",
                    "abandoned",
                    "crushed",
                    "overwhelmed",
                    "devastated",
                    "gratified",
                    "upset",
                    "indifferent",
                    "despair",
                    "determined" 
                }; //  a hug list of human emotions
                var emotionParameters = new Dictionary<string, ParameterDefinition>
                {
                    { "emotion", new ParameterDefinition{ Type = "string", Description = "the emotion the character should display", Enum = emotionsOptions } },
                };
                var emotionTool = ToolCreator.CreateTool("ChooseEmotion", "Choose the desired emotion for the situation", emotionParameters, new List<string> { "emotion" });
                Debug.Log(emotionTool);

                // Example Action Tool to Json
                var searchParameters = new Dictionary<string, ParameterDefinition>
                {
                    { "character", new ParameterDefinition{Type = "string", Description = "The character to find the object." } },
                    { "objectToFind", new ParameterDefinition{Type = "string", Description = "The object to search for." } },
                };
                var searchTool = ToolCreator.CreateTool("Search", "Tool to search for an object in the environment", searchParameters, new List<string> { "character", "objectToFind" });
                Debug.Log(searchTool);

                var toolset = new List<Tool> { emotionTool, searchTool };

                // Create the function message
                var functionMessage = new FunctionMessage { history = history, tools = toolset };
                Debug.Log(JsonConvert.SerializeObject(functionMessage, Formatting.Indented));
                */

            try
            {
                //var chatAsyncResponse = await CallEndpointPostAsync<ChatCompletion.Response>(chatAsyncUrl, chatmessage);
                //var imageAsyncResponse = await CallEndpointPostAsync<Image.Response>(imageAsyncsUrl, imagePrompt);
                //var visionUrlResponse = await CallEndpointPostAsync<ChatCompletion.Response>(visionUrl, visionmessage.url = imageAsyncResponse.Data[0].Url);
                //var visionBytesResponse = await CallEndpointPostAsync<ChatCompletion.Response>(visionBytesUrl, visionbytesmessage);
                //var speechToTextAsyncTimeStampResponse = await CallEndpointPostAsync<SpeechToText.TimeStampResponse>(speechToTextTimeStampUrl, speechtotextmessage);
                var speechToTextAsyncResponse = await CallEndpointPostAsync<SpeechToText.Response>(speeachToTextUrl, speechtotextmessage);
                //var moderationsAsyncResponse = await CallEndpointPostAsync<Moderation.Response>(moderationsAsyncUrl, chatmessage);
                //var assistantResponse = await CallEndpointPostAsync<List<Assistant.Response>>(assistantUrl, assistantmessage);
                //var functionResponse = await CallEndpointPostAsync<ChatCompletion.Response>(functionsUrl, functionMessage);

                //Debug.Log(chatAsyncResponse.Choices[0].Message.Content);
                //Debug.Log(imageAsyncResponse.Data[0].Url);
                //Debug.Log(visionUrlResponse.Choices[0].Message.Content);

                //Debug.Log(visionBytesResponse.Choices[0].Message.Content);
                //Debug.Log(speechToTextAsyncTimeStampResponse.Words);
                Debug.Log(speechToTextAsyncResponse.Text);
                //Debug.Log(moderationsAsyncResponse.Results[0].Flagged);

                //Debug.Log(assistantResponse[0].Content[0].Text.Value);

                /*
                // Parse function call responses
                foreach (var choice in functionResponse.Choices)
                {
                    Debug.Log(choice.Message.Content); // Chat response

                    foreach (var toolCall in choice.Message.ToolCalls)
                    {
                         if (toolCall.Function.Name == "ChooseEmotion")
                         {
                            //Debug.Log(toolCall.Function.Name);
                            //Debug.Log(toolCall.Function.Arguments); // Emotion choice
                            var emote = JsonConvert.DeserializeObject<Emote>(toolCall.Function.Arguments);
                            Emotive(emote.emotion);
                         }
                         else if (toolCall.Function.Name == "Search")
                         {
                            Debug.Log(toolCall.Function.Name);
                            Debug.Log(toolCall.Function.Arguments);
                            var find = JsonConvert.DeserializeObject<Find>(toolCall.Function.Arguments);
                            Search(find);
                         }
                    }   
                }

                //Debug.Log(functionResponse.Choices[0].Message.ToolCalls[0].Function.Name); // Function that is called
                */
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error calling API: {ex.Message}");
            }
        }


        // Get
        public static async Task<T> CallEndpointGetAsync<T>(string url)
        {
            try
            {
                using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
                {
                    var result = await webRequest.SendWebRequest().AsTask();

                    if (result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Error: {webRequest.error}");
                        return default(T); // Return null or a default instance depending on your error handling strategy
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"An exception occurred: {ex.Message}");
                return default(T); // Return null or a default instance depending on your error handling strategy
            }
        }

        // Post
        public static async Task<T> CallEndpointPostAsync<T>(string url, object data)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

                using (var request = new UnityWebRequest(url, "POST"))
                {
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");

                    var operation = request.SendWebRequest();

                    while (!operation.isDone)
                    {
                        await Task.Yield();
                    }

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Error: {request.error}");
                        return default(T);
                    }
                    else
                    {
                        string responseJson = request.downloadHandler.text;
                        T result = JsonConvert.DeserializeObject<T>(responseJson);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message}");
                return default(T);
            }
        }
    }
}