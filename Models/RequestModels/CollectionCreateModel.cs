using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Models.RequestModels
{
    public class CollectionCreateModel
    {
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
