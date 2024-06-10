namespace CourseProject_backend.Models.ViewModels
{
    public class JiraTicketViewModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LangPack { get; set; }

        public string Collection {  get; set; }

        public string Link {  get; set; }
    }
}
