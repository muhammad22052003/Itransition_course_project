using CourseProject_backend.Entities;
using CourseProject_backend.Enums;

namespace CourseProject_backend.Models.ViewModels
{
    public class ItemsViewModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LanguagePack {  get; set; }

        public List<Item> Items {  get; set; }

        public int CurrentPage {  get; set; }

        public int PagesCount { get; set; }

        public ItemsDataFilter Filter {  get; set; }

        public DataSort Sort {  get; set; }
        public string Value {  get; set; }
    }
}
