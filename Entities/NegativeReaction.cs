using CourseProject_backend.Entities.interfaces;
using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Entities
{
    public class NegativeReaction : IReaction
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public User User { get; set; }
        public Item Item { get; set; }
    }
}
