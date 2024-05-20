using CourseProject_backend.Entities;

namespace CourseProject_backend.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<MyCollection> Collections { get; set; }
        public List<Item> Items { get; set; }

        public KeyValuePair<string, IDictionary<string, string>> LanguagePack { get; set; }
    }
}
