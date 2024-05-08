using CourseProject_backend.Entities.interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_backend.Entities
{
    [Table("collections")]
    [Index(nameof(CreatedTime))]
    public class MyCollection : IMyCollection
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool CustomString1_state { get; set; }
        public string CustomString1_name { get; set; }
        public bool CustomString2_state { get; set; }
        public string CustomString2_name { get; set; }
        public bool CustomString3_state { get; set; }
        public string CustomString3_name { get; set; }
        public bool CustomText1_state { get; set; }
        public string CustomText1_name { get; set; }
        public bool CustomText2_state { get; set; }
        public string CustomText2_name { get; set; }
        public bool CustomText3_state { get; set; }
        public string CustomText3_name { get; set; }
        public bool CustomInt1_state { get; set; }
        public string CustomInt1_name { get; set; }
        public bool CustomInt2_state { get; set; }
        public string CustomInt2_name { get; set; }
        public bool CustomInt3_state { get; set; }
        public string CustomInt3_name { get; set; }
        public bool CustomBool1_state { get; set; }
        public string CustomBool1_name { get; set; }
        public bool CustomBool2_state { get; set; }
        public string CustomBool2_name { get; set; }
        public bool CustomBool3_state { get; set; }
        public string CustomBool3_name { get; set; }
    }
}
