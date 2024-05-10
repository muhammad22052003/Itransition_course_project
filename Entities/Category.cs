using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Name))]
    [Index(nameof(IsDeleted))]
    public class Category : IDBModel
    {
        public Category()
        {
            
        }

        public Category(string name, string desc)
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
            Name = name;
            Description = desc;
            IsDeleted = false;
        }

        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public ICollection<MyCollection> Collections {  get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
