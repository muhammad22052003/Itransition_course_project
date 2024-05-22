using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Models.RequestModels
{
    public class EditProfileModel
    {
        [Required(ErrorMessage = "Data in the field must be defined")]
        public string UserId {  get; set; }
        [Required(ErrorMessage = "Data in the field must be defined")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Data in the field must be defined")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Data in the field must be defined")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Data in the field must be defined")]
        [StringLength(maximumLength: 40, MinimumLength = 4)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
