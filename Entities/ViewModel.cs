using CourseProject_backend.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CourseProject_backend.Entities
{
    public class ViewModel : IDBModel
    {
        public ViewModel(User user, Item item)
        {
            Id = Guid.NewGuid().ToString().Replace("-","");
            User = user;
            Item = item;
            ViewTime = DateTime.UtcNow;
        }

        public ViewModel() { }

        [Key]
        public string Id { get; set; }
        
        public User? User { get; set; }
        [Required]
        public Item Item { get; set; }
        [Required]
        public DateTime ViewTime {  get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
