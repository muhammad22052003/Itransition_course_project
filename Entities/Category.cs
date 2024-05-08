using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Name))]
    [Index(nameof(IsDeleted))]
    public class Category : IDBModel
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
