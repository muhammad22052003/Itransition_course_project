using System.Text.Json.Serialization;

namespace CustomJiraTicketClient.Jira.Models.Field
{
    public class EnumType
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}