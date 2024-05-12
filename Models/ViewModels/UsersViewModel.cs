using CourseProject_backend.Entities;
using CourseProject_backend.Enums;

namespace CourseProject_backend.Models.ViewModels
{
    public class UsersViewModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LanguagePack {  get; set; }

        public List<User> Items {  get; set; }

        public int CurrentPage {  get; set; }

        public int PagesCount { get; set; }

        public UsersDataFilter Filter {  get; set; }

        public DataSort Sort {  get; set; }
        public string Value {  get; set; }
    }
}
