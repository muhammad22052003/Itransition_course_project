using CourseProject_backend.Entities;
using CourseProject_backend.Enums;

namespace CourseProject_backend.Models.ViewModels
{
    public class ItemConstructorViewModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LanguagePack { get; set; }

        public MyCollection Collection { get; set; }
    }
}
