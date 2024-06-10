using CourseProject_backend.Entities;
using CustomJiraTicketClient.Jira.Models;

namespace CourseProject_backend.Models.ViewModels
{
    public class MyTicketsViewModel
    {
        public KeyValuePair<string, IDictionary<string,string>> LangPack { get; set; }

        public List<JiraTicket> Tickets { get; set; }

        public User User { get; set; }

        public int CurrentPage {  get; set; }

        public int PageSize { get; set; }  

        public string BaseTicketsUrl {  get; set; }
    }
}
