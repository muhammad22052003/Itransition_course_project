using System.Text.Json.Serialization;

namespace CustomJiraTicketClient.Jira.Models.Field
{
    public class ParagraphType
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("content")]
        public List<ParagraphType> Content { get; set; }
    }
}
