using CustomJiraTicketClient.Jira.Models.Field;
using CustomJiraTicketClient.Jira.Models.FieldTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomJiraTicketClient.Jira.Models
{
    public class JiraTicket
    {

        public JiraTicket()
        {
            
        }

        public JiraTicket
        (
            string summary,
            JiraUser reporter,
            string description,
            string prioritet,
            string collection,
            string link,
            string projectKey
        )
        {
            Summary = summary;
            Reporter = new JiraUser() { AccounId = reporter.AccounId, Email = reporter.Email };
            Description = description;
            Prioritet = new EnumType() { Value = prioritet };
            Collection = collection;
            Link = link;
            Created = DateTime.Now.Date.ToString("yyyy-MM-dd");
            Project = new ProjectType() { Key = projectKey };
            Type = new IssueType() { Id = "10011" };
        }
        public string Id { get; set; }

        public string Key { get; set; }

        public string Summary { get; set; }

        public JiraUser Reporter { get; set; }

        public string? Description { get; set; }

        public EnumType Prioritet { get; set; }

        public string Self {  get; set; }

        public string Collection { get; set; }
        public string Link { get; set; }
        public string Created { get; set; }
        public ProjectType Project { get; set; }
        public IssueType Type { get; set; }
        public string Status { get; set; }
    }
}
