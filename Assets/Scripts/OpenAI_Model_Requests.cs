using System;
using System.Collections.Generic;
using static Indie.OpenAI.Tools.ToolCreator;

namespace Indie.OpenAI.Models.Requests
{
    // Chat
    [Serializable]
    public class ChatMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    [Serializable]
    public class ChatHistory
    {
        public List<ChatMessage> messages { get; set; } = new List<ChatMessage>();
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

    // Assistant
    public class AssistantMessage
    {
        public string input { get; set; }
        public string assistant_id { get; set; }
    }
}
