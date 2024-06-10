using System.Text.Json.Serialization;

namespace CustomJiraTicketClient.Jira.Models.FieldTypes
{
    public class JiraUser
    {
        [JsonPropertyName(name: "accountId")]
        public string AccounId { get; set; }
        [JsonPropertyName(name: "displayName")]
        public string DisplayName { get; set; }
        [JsonPropertyName(name: "emailAddress")]
        public string Email { get; set; }
        [JsonPropertyName(name: "active")]
        public bool IsActive { get; set; }
        [JsonPropertyName(name: "timeZone")]
        public string TimeZone { get; set; }
    }
}
