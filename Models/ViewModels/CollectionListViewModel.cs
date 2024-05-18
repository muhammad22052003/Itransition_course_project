using CourseProject_backend.Entities;
using CourseProject_backend.Enums;

namespace CourseProject_backend.Models.ViewModels
{
    public class CollectionListViewModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LanguagePack {  get; set; }

        public List<MyCollection> Collections {  get; set; }

        public User User { get; set; }

        public int CurrentPage {  get; set; }

        public int PagesCount { get; set; }

        public CollectionDataFilter Filter {  get; set; }

        public DataSort Sort {  get; set; }
        public string Value {  get; set; }
    }
}
