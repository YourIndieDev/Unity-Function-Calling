using System;
using System.Collections.Generic;
using static Indie.OpenAI.Tools.ToolCreator;

namespace Indie.OpenAI.Models.Requests
{
    // Chat
    [Serializable]
    public class ChatMessage
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class ChatHistory
    {
        public List<ChatMessage> messages = new List<ChatMessage>();
    }


    // Functions/Tools
    public class FunctionMessage : ChatHistory
    {
        public List<Tool> tools { get; set; }
    }

    public class FunctionMessageToolless
    {
        public List<ChatMessage> messages { get; set; }
        //public List<Tool> tools { get; set; }
    }


    // Vision
    public class VisionMessage
    {
        public string url { get; set; }
        public string input { get; set; }
    }

    public class VisionBytesMessage
    {
        public string image_path { get; set; }
        public string input { get; set; }
    }

    // Speech to Text
    public class SpeechToTextMessage
    {
        public string audio_path { get; set; }
    }

    // Text to Speech
    public class TextToSpeechMessage
    {
        public string content { get; set; }
        public string voice { get; set; } = "alloy";
    }

    // Assistant
    public class AssistantMessage
    {
        public string input { get; set; }
        public string assistant_id { get; set; }
    }
}
