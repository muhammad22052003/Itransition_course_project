using CourseProject_backend.Enums;
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
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public DateTime RegistrationTime { get; set; }
        public List<ViewModel>? Views { get; set; }
        [Required]
        public List<Comment> Comments { get; set; }
        [Required]
        public List<PositiveReaction> PositiveReactions { get; set; }
        [Required]
        public List<MyCollection> Collections { get; set; }
        [Required]
        public bool IsBlocked {  get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
