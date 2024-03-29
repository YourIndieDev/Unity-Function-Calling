using System.Collections.Generic;
using Newtonsoft.Json;


namespace Indie.OpenAI.Models.Responses
{
    // Chat
    public class ChatCompletion
    {
        public class Response
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("choices")]
            public List<Choice> Choices { get; set; }

            [JsonProperty("created")]
            public long Created { get; set; }

            [JsonProperty("model")]
            public string Model { get; set; }

            [JsonProperty("object")]
            public string Object { get; set; }

            [JsonProperty("system_fingerprint")]
            public string SystemFingerprint { get; set; }

            [JsonProperty("usage")]
            public Usage Usage { get; set; }
        }

        public class Choice
        {
            [JsonProperty("finish_reason")]
            public string FinishReason { get; set; }

            [JsonProperty("index")]
            public int Index { get; set; }

            [JsonProperty("logprobs")]
            public object Logprobs { get; set; } // Use appropriate type or leave as object if unknown

            [JsonProperty("message")]
            public Message Message { get; set; }
        }

        public class Message
        {
            [JsonProperty("content")]
            public string Content { get; set; }

            [JsonProperty("role")]
            public string Role { get; set; }

            [JsonProperty("function_call")]
            public Function FunctionCall { get; set; } // Use appropriate type or leave as object if unknown

            [JsonProperty("tool_calls")]
            public List<ToolCall> ToolCalls { get; set; } // Use appropriate type or leave as object if unknown
        }

        public class ToolCall
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("function")]
            public Function Function { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class Function
        {
            [JsonProperty("arguments")]
            public string Arguments { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Usage
        {
            [JsonProperty("completion_tokens")]
            public int CompletionTokens { get; set; }

            [JsonProperty("prompt_tokens")]
            public int PromptTokens { get; set; }

            [JsonProperty("total_tokens")]
            public int TotalTokens { get; set; }
        }
    }

    // Image
    public class Image
    {
        public class Response
        {
            [JsonProperty("created")]
            public long Created { get; set; }

            [JsonProperty("data")]
            public List<Data> Data { get; set; }
        }

        public class Data
        {
            [JsonProperty("b64_json")]
            public object B64Json { get; set; }

            [JsonProperty("revised_prompt")]
            public string RevisedPrompt { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }
    }

    // Speech To Text
    public class SpeechToText
    {
        public class TimeStampResponse
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("task")]
            public string Task { get; set; }

            [JsonProperty("language")]
            public string Language { get; set; }

            [JsonProperty("duration")]
            public double Duration { get; set; }

            [JsonProperty("words")]
            public List<Word> Words { get; set; }
        }

        public class Word
        {
            [JsonProperty("word")]
            public string Text { get; set; }

            [JsonProperty("start")]
            public double Start { get; set; }

            [JsonProperty("end")]
            public double End { get; set; }
        }

        public class Response
        {
            [JsonProperty("text")]
            public string Text { get; set; }
        }
    }

    // Moderations
    public class Moderation
    {
        public class Response
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("model")]
            public string Model { get; set; }

            [JsonProperty("results")]
            public List<Result> Results { get; set; }
        }

        public class Result
        {
            [JsonProperty("flagged")]
            public bool Flagged { get; set; }

            [JsonProperty("categories")]
            public Categories Categories { get; set; }

            [JsonProperty("category_scores")]
            public CategoryScores CategoryScores { get; set; }
        }

        public class Categories
        {
            [JsonProperty("sexual")]
            public bool Sexual { get; set; }

            [JsonProperty("hate")]
            public bool Hate { get; set; }

            [JsonProperty("harassment")]
            public bool Harassment { get; set; }

            [JsonProperty("self-harm")]
            public bool SelfHarm { get; set; }

            [JsonProperty("sexual/minors")]
            public bool SexualMinors { get; set; }

            [JsonProperty("hate/threatening")]
            public bool HateThreatening { get; set; }

            [JsonProperty("violence/graphic")]
            public bool ViolenceGraphic { get; set; }

            [JsonProperty("self-harm/intent")]
            public bool SelfHarmIntent { get; set; }

            [JsonProperty("self-harm/instructions")]
            public bool SelfHarmInstructions { get; set; }

            [JsonProperty("harassment/threatening")]
            public bool HarassmentThreatening { get; set; }

            [JsonProperty("violence")]
            public bool Violence { get; set; }
        }

        public class CategoryScores
        {
            [JsonProperty("sexual")]
            public double Sexual { get; set; }

            [JsonProperty("hate")]
            public double Hate { get; set; }

            [JsonProperty("harassment")]
            public double Harassment { get; set; }

            [JsonProperty("self-harm")]
            public double SelfHarm { get; set; }

            [JsonProperty("sexual/minors")]
            public double SexualMinors { get; set; }

            [JsonProperty("hate/threatening")]
            public double HateThreatening { get; set; }

            [JsonProperty("violence/graphic")]
            public double ViolenceGraphic { get; set; }

            [JsonProperty("self-harm/intent")]
            public double SelfHarmIntent { get; set; }

            [JsonProperty("self-harm/instructions")]
            public double SelfHarmInstructions { get; set; }

            [JsonProperty("harassment/threatening")]
            public double HarassmentThreatening { get; set; }

            [JsonProperty("violence")]
            public double Violence { get; set; }
        }
    }

    // Assistant
    public class Assistant
    {
        public class Response
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("assistant_id")]
            public string AssistantId { get; set; }

            [JsonProperty("content")]
            public List<Content> Content { get; set; }

            [JsonProperty("created_at")]
            public long CreatedAt { get; set; }

            [JsonProperty("file_ids")]
            public List<object> FileIds { get; set; }

            [JsonProperty("metadata")]
            public Dictionary<string, object> Metadata { get; set; }

            [JsonProperty("object")]
            public string Object { get; set; }

            [JsonProperty("role")]
            public string Role { get; set; }

            [JsonProperty("run_id")]
            public string RunId { get; set; }

            [JsonProperty("thread_id")]
            public string ThreadId { get; set; }
        }

        public class Content
        {
            [JsonProperty("text")]
            public Text Text { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class Text
        {
            [JsonProperty("annotations")]
            public List<object> Annotations { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }

        public class ResponseList
        {
            [JsonProperty("responses")]
            public List<Response> Responses { get; set; }
        }
    }
}