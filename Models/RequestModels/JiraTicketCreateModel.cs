using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Models.RequestModels
{
    public class JiraTicketCreateModel
    {
        [Required]
        public string Summary {  get; set; }
        [Required]
        public string Description {  get; set; }
        [Required]
        public string Prioritet {  get; set; }
        [Required]
        public string Link {  get; set; }

        public string Collection {  get; set; }
    }
}
