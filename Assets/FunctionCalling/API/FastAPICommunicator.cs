using Indie.OpenAI.Models.Requests;
using Indie.OpenAI.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


namespace Indie.OpenAI.API 
{
    public static class UnityWebRequestExtensions
    {
        /// <summary>
        /// Converts a UnityWebRequestAsyncOperation to a Task that represents its completion.
        /// </summary>
        /// <param name="asyncOp">The UnityWebRequestAsyncOperation to convert.</param>
        /// <returns>A Task representing the completion of the UnityWebRequestAsyncOperation.</returns>
        public static Task<UnityWebRequest.Result> AsTask(this UnityWebRequestAsyncOperation asyncOp)
        {
            var completionSource = new TaskCompletionSource<UnityWebRequest.Result>();
            asyncOp.completed += _ => completionSource.TrySetResult(asyncOp.webRequest.result);
            return completionSource.Task;
        }
    }

    public static class FastAPICommunicator
    {
        // Chat URLS
        public const string baseLocalHostUrl = "http://localhost:8000/";
        public const string baseUrl = "https://ai-python-server.onrender.com/";

        public static string chatAsyncUrl = $"{baseLocalHostUrl}chat_async/";
        public static string chatHistoryAsyncUrl = $"{baseLocalHostUrl}chat_history_async/";
        public static string chatMessageAsyncUrl = $"{baseLocalHostUrl}chat_message_async/";
        public static string functionsUrl = $"{baseLocalHostUrl}chat_functions/";

        public static string visionUrl = $"{baseLocalHostUrl}vision_async_url/";
        public static string visionBytesUrl = $"{baseLocalHostUrl}vision_async_bytes/";

        public static string imageAsyncsUrl = $"{baseLocalHostUrl}image_async/";

        public static string speechToTextTimePathStampUrl = $"{baseLocalHostUrl}speech_to_text_path_timestamp_async/";
        public static string speechToTextPathUrl = $"{baseLocalHostUrl}speech_to_text_path_async/";
        public static string speechToTextDataUrl = $"{baseLocalHostUrl}speech_to_text_data_async/";


        public static string textToSpeechUrl = $"{baseLocalHostUrl}text_to_speech_async/";

        public static string moderationsAsyncUrl = $"{baseLocalHostUrl}moderations_async/";

        public static string assistantUrl = $"{baseLocalHostUrl}assistant/";


        // Examples of calling the API
        /*
        public static async void CallExamples()
        {
            var chatHistory = new ChatHistory();
            chatHistory.messages.Add( new ChatMessage { role ="user", content = "Remember the number 333" });
            chatHistory.messages.Add( new ChatMessage { role ="assistant", content = "Okay" });
            chatHistory.messages.Add( new ChatMessage { role ="user", content = "What is the number i asked you to rememeber?" });

            var visionmessage = new VisionMessage { url = "", input = "Explain this" };
            var visionbytesmessage = new VisionBytesMessage { image_path = "", input = "Explain this" };
            var speechtotextmessage = new SpeechToTextMessage { audio_path = @"C:\\Users\RecallableFacts\Desktop\ElevenLabs_2024-02-26T07_20_33_NarratorDano_ivc_s76_sb75_se50_b_m2.mp3" };
            var assistantmessage = new AssistantMessage { input = "Create a story of An astronaut exploring a lush, alien jungle filled with towering, glowing mushrooms and mysterious, bioluminescent creatures.", assistant_id = "asst_ZE2w0PwhCO6XWJk7rZB1xAcD"};
            var imagePrompt = new ChatMessage { content = "An astronaut exploring a lush, alien jungle filled with towering, glowing mushrooms and mysterious, bioluminescent creatures." };

            ChatMessage chatmessage = new ChatMessage { content = "What is the sum of 10 and 12" };
            FunctionMessage functionMessage = new FunctionMessage { messages = chatHistory.messages };

            try
            {
                var chatMessageAsyncResponse = await CallEndpointPostAsync<ChatCompletion.Response>(chatMessageAsyncUrl, chatmessage);
                var chatHistoryAsyncResponse = await CallEndpointPostAsync<ChatCompletion.Response>(chatHistoryAsyncUrl, chatHistory);
                var imageAsyncResponse = await CallEndpointPostAsync<Image.Response>(imageAsyncsUrl, imagePrompt);
                var visionUrlResponse = await CallEndpointPostAsync<ChatCompletion.Response>(visionUrl, visionmessage.url = imageAsyncResponse.Data[0].Url);
                var visionBytesResponse = await CallEndpointPostAsync<ChatCompletion.Response>(visionBytesUrl, visionbytesmessage);
                var speechToTextAsyncTimeStampResponse = await CallEndpointPostAsync<SpeechToText.TimeStampResponse>(speechToTextTimeStampUrl, speechtotextmessage);
                var speechToTextAsyncResponse = await CallEndpointPostAsync<SpeechToText.Response>(speechToTextUrl, speechtotextmessage);
                var moderationsAsyncResponse = await CallEndpointPostAsync<Moderation.Response>(moderationsAsyncUrl, chatmessage);
                var assistantResponse = await CallEndpointPostAsync<List<Assistant.Response>>(assistantUrl, assistantmessage);
                var functionResponse = await CallEndpointPostAsync<ChatCompletion.Response>(functionsUrl, functionMessage);

                Debug.Log(chatMessageAsyncResponse.Choices[0].Message.Content);
                Debug.Log(chatHistoryAsyncResponse.Choices[0].Message.Content);
                Debug.Log(imageAsyncResponse.Data[0].Url);
                Debug.Log(visionUrlResponse.Choices[0].Message.Content);

                Debug.Log(visionBytesResponse.Choices[0].Message.Content);
                Debug.Log(speechToTextAsyncTimeStampResponse.Words);
                Debug.Log(speechToTextAsyncResponse.Text);
                Debug.Log(moderationsAsyncResponse.Results[0].Flagged);

                Debug.Log(assistantResponse[0].Content[0].Text.Value);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error calling API: {ex.Message}");
            }
        }
        */


        /// <summary>
        /// Sends a GET request to the specified URL and returns the response deserialized as the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the response to.</typeparam>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <returns>The deserialized response object of type <typeparamref name="T"/>.</returns>
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

        /// <summary>
        /// Sends a POST request to the specified URL with the provided data, and returns the response deserialized as the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the response to.</typeparam>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="data">The data to send with the POST request.</param>
        /// <param name="header">The content type header for the request (default is "application/json").</param>
        /// <returns>The deserialized response object of type <typeparamref name="T"/>.</returns>
        public static async Task<T> CallEndpointPostAsync<T>(string url, object data, string header = "application/json")
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

                using (var request = new UnityWebRequest(url, "POST"))
                {
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", header);

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

        /// <summary>
        /// Sends a POST request to the specified URL with the provided data and returns the response as a byte array.
        /// </summary>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="data">The data to send with the POST request.</param>
        /// <param name="header">The content type header for the request (default is "application/json").</param>
        /// <returns>The response as a byte array.</returns>
        public static async Task<byte[]> CallEndpointPostAsyncForBytes(string url, object data, string header = "application/json")
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

                using (var request = new UnityWebRequest(url, "POST"))
                {
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", header);

                    var operation = request.SendWebRequest();

                    while (!operation.isDone)
                    {
                        await Task.Yield();
                    }

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Error: {request.error}");
                        return null;
                    }
                    else
                    {
                        // Directly return the byte array for binary data
                        return request.downloadHandler.data;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message}");
                return null;
            }
        }
    }
}