using CourseProject_backend.Entities.interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Text))]
    public class Comment : IComment
    {
        [Key]
        public string Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public Item Item { get; set; }
    }
}
