using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CustomJiraTicketClient.Jira;
using CustomJiraTicketClient.Jira.Models.FieldTypes;
using CustomJiraTicketClient.Models.FieldTypes;

namespace CustomJiraTicketClient.Jira.Models.Field
{
    public class TicketFields
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("customfield_10053")]
        public EnumType Prioritet { get; set; }

        [JsonPropertyName("customfield_10050")]
        public List<JiraUser> User { get; set; }
        [JsonPropertyName("customfield_10056")]
        public string Collection { get; set; }
        [JsonPropertyName("customfield_10052")]
        public string Link { get; set; }
        [JsonPropertyName("description")]
        public ParagraphType Description { get; set; }
        [JsonPropertyName("customfield_10015")]
        public string Created { get; set; }
        [JsonPropertyName("project")]
        public ProjectType Project { get; set; }
        [JsonPropertyName("issuetype")]
        public IssueType Type { get; set; }
        [JsonPropertyName("status")]
        public StatusType Status { get; set; }
    }
}
