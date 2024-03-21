using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;


namespace AI
{
    public class Context_UI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public Color normalColor = Color.white;
        public Color objectKeyColor = Color.blue;
        public Color numberColor = Color.green;
        public Color stringColor = Color.red;
        public Color booleanColor = Color.cyan;
        public Color nullColor = Color.gray;
        public Color arrayColor = Color.magenta;

        private void OnEnable()
        {
            ContextManager.OnSerializedConext += FormatJson;
        }

        private void OnDisable()
        {
            ContextManager.OnSerializedConext -= FormatJson;
        }


        // Helper method to convert a Color to a hex string
        private string ColorToHex(Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }

        // Method to colorize and indent JSON
        public void FormatJson(string json)
        {
            var parsedJson = JToken.Parse(json);
            string formattedJson = ColorizeAndIndentJson(parsedJson);
            text.text = formattedJson;
        }

        private string ColorizeAndIndentJson(JToken token, int indentLevel = 0)
        {
            var builder = new System.Text.StringBuilder();
            var indent = new string(' ', indentLevel * 4);

            switch (token.Type)
            {
                case JTokenType.Object:
                    AppendObject(token, builder, indent, indentLevel);
                    break;
                case JTokenType.Array:
                    AppendArray(token, builder, indent, indentLevel);
                    break;
                case JTokenType.Integer:
                case JTokenType.Float:
                    builder.Append($"{indent}<color=#{ColorToHex(numberColor)}>{token}</color>\n");
                    break;
                case JTokenType.String:
                    string str = token.ToString();
                    builder.Append($"{indent}<color=#{ColorToHex(stringColor)}>\"{str}\"</color>\n");
                    break;
                case JTokenType.Boolean:
                    builder.Append($"{indent}<color=#{ColorToHex(booleanColor)}>{token}</color>\n");
                    break;
                case JTokenType.Null:
                    builder.Append($"{indent}<color=#{ColorToHex(nullColor)}>null</color>\n");
                    break;
                default:
                    builder.Append($"{indent}<color=#{ColorToHex(normalColor)}>{token}</color>\n");
                    break;
            }
            return builder.ToString();
        }

        private void AppendObject(JToken token, System.Text.StringBuilder builder, string indent, int indentLevel)
        {
            builder.AppendLine($"{indent}{{");
            foreach (var child in token.Children<JProperty>())
            {
                builder.Append($"{indent}    <color=#{ColorToHex(objectKeyColor)}>");
                builder.Append($"\"{child.Name}\"</color>: ");
                builder.Append(ColorizeAndIndentJson(child.Value, indentLevel));
            }
            builder.AppendLine($"{indent}}}");
        }

        private void AppendArray(JToken token, System.Text.StringBuilder builder, string indent, int indentLevel)
        {
            builder.AppendLine($"{indent}[");
            foreach (var child in token.Children())
            {
                builder.Append(ColorizeAndIndentJson(child, indentLevel + 1));
            }
            builder.AppendLine($"{indent}]");
        }
    }
}
