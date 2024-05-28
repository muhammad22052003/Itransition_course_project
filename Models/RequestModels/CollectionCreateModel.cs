using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Models.RequestModels
{
    public class CollectionCreateModel
    {
        public string? CollectionId { get; set; }

        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? CustomString1_name { get; set; }
        public string? CustomString2_name { get; set; }
        public string? CustomString3_name { get; set; }
        public string? CustomText1_name { get; set; }
        public string? CustomText2_name { get; set; }
        public string? CustomText3_name { get; set; }
        public string? CustomInt1_name { get; set; }
        public string? CustomInt2_name { get; set; }
        public string? CustomInt3_name { get; set; }
        public string? CustomBool1_name { get; set; }
        public string? CustomBool2_name { get; set; }
        public string? CustomBool3_name { get; set; }
        public string? CustomDate1_name { get; set; }
        public string? CustomDate2_name { get; set; }
        public string? CustomDate3_name { get; set; }
    }
}
