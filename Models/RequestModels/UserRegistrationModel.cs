using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Models.RequestModels
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Data in the field must be defined")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Data in the field must be defined")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Data in the field must be defined")]
        [RegularExpression(@"[A-Za-z0-9.%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Uncorrect Email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Data in the field must be defined")]
        [StringLength(maximumLength : 40,MinimumLength = 4)]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
