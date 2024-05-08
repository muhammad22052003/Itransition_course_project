using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Name))]
    public class Tag : IDBModel
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public Item Item { get; set; }
    }
}
