using System.Collections.Generic;

namespace UnityCopilot.Models
{
    [System.Serializable]
    public class ChatMessage
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class ChatInputModel
    {
        public string userMessage;
        public List<ChatMessage> chatHistory;
    }
}
