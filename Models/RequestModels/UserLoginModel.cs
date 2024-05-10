using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Models.RequestModels
{
    public class UserLoginModel
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9.%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength : 40,MinimumLength = 4)]
        public string Password { get; set; }
    }
}
