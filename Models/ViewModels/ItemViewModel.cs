using CourseProject_backend.Entities;

namespace CourseProject_backend.Models.ViewModels
{
    public class ItemViewModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LanguagePack { get; set; }

        public Item Item { get; set; }

        public User User { get; set; }
    }
}
