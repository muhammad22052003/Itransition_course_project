using CourseProject_backend.Entities;
using System.ComponentModel.DataAnnotations;

namespace CourseProject_backend.Models.RequestModels
{
    public class ItemPostModel
    {
        public string? Id { get; set; }  
        [Required]
        public string CollectionId {  get; set; }
        [Required]
        public string Name { get; set; }

        public string? Tags { get; set; }

        public string? CustomString1 { get; set; }
        public string? CustomString2 { get; set; }
        public string? CustomString3 { get; set; }
        public string? CustomText1 { get; set; }
        public string? CustomText2 { get; set; }
        public string? CustomText3 { get; set; }
        public int? CustomInt1 { get; set; }
        public int? CustomInt2 { get; set; }
        public int? CustomInt3 { get; set; }
        public bool? CustomBool1 { get; set; }
        public bool? CustomBool2 { get; set; }
        public bool? CustomBool3 { get; set; }
        public DateTime? CustomDate1 { get; set; }
        public DateTime? CustomDate2 { get; set; }
        public DateTime? CustomDate3 { get; set; }
    }
}
