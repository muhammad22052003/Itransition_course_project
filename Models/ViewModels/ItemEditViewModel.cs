using CourseProject_backend.Entities;
using CourseProject_backend.Enums;

namespace CourseProject_backend.Models.ViewModels
{
    public class ItemEditViewModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LanguagePack { get; set; }

        public Item Item { get; set; }
    }
}
