using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Text))]
    public class Comment : IDBModel
    {
        public Comment()
        {
            
        }

        public Comment(string text, User user, Item item)
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
            Text = text;
            User = user;
            Item = item;
        }

        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Item Item { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
