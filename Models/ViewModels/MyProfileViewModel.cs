using CourseProject_backend.Entities;

namespace CourseProject_backend.Models.ViewModels
{
    public class MyProfileViewModel
    {
        public User User {  get; set; }

        public KeyValuePair<string, IDictionary<string, string>> LanguagePack { get; set; }
    }
}
