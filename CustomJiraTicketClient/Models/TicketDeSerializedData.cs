using CustomJiraTicketClient.Jira.Models.Field;
using CustomJiraTicketClient.Jira.Models.FieldTypes;
using System.Text.Json.Serialization;

namespace CustomJiraTicketClient.Jira.Models
{
    public class TicketDeSerializedData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("fields")]
        public TicketFields Fields { get; set; }

        [JsonPropertyName("self")]
        public string Self {  get; set; }

        public JiraTicket GetTicket()
        {
            JiraTicket ticket = new JiraTicket();

            ticket.Id = Id;
            ticket.Key = Key;
            ticket.Summary = Fields.Summary;
            ticket.Reporter = Fields.User?.FirstOrDefault();
            ticket.Description = Fields.Description.Content?.FirstOrDefault()?.Content.FirstOrDefault()?.Text;
            ticket.Prioritet = Fields.Prioritet;
            ticket.Collection = Fields.Collection;
            ticket.Link = Fields.Link;
            ticket.Created = Fields.Created;
            ticket.Project = Fields.Project;
            ticket.Type = Fields.Type;
            ticket.Status = Fields.Status.Name;
            ticket.Self = Self;

            return ticket;
        }
    }
}
