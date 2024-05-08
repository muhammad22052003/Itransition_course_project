using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Text))]
    public class Comment : IDBModel
    {
        [Key]
        public string Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public Item Item { get; set; }
    }
}
