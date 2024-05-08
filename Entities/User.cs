using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Entities
{
    [Index(nameof(Email))]
    [Index(nameof(Role))]
    public class User : IDBModel
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime RegistrationTime { get; set; }
        public List<Comment> Comments { get; set; }
        public List<PositiveReaction> PositiveReactions { get; set; }
        public List<NegativeReaction> NegativeReactions { get; set; }
    }
}
