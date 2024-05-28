using CourseProject_backend.Entities;

namespace CourseProject_backend.Models.ViewModels
{
    public class CollectionConstructorViewModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LanguagePack { get; set; }

        public MyCollection Collection { get; set; }
    }
}
