namespace CourseProject_backend.Models.ViewModels
{
    public class MessageViewModel
    {
        public KeyValuePair<string, IDictionary<string,string>> LangPack { get; set; }

        public string Message {  get; set; }
    }
}
