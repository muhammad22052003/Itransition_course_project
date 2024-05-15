using CourseProject_backend.Entities;

namespace CourseProject_backend.Models.ViewModels
{
    public class CabinetViewModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LanguagePack { get; set; }

        public User User { get; set; }

        public List<MyCollection> Collections { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
