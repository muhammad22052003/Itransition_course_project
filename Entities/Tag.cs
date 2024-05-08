using CourseProject_backend.Entities.interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Name))]
    public class Tag : ITag
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public Item Item { get; set; }
    }
}
