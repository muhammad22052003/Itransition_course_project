using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_backend.Entities
{
    [Table("collections")]
    [Index(nameof(CreatedTime))]
    public class MyCollection : IDBModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public ICollection<Item> Items { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }
        public bool CustomString1_state { get; set; }
        public string? CustomString1_name { get; set; }
        public bool CustomString2_state { get; set; }
        public string? CustomString2_name { get; set; }
        public bool CustomString3_state { get; set; }
        public string? CustomString3_name { get; set; }
        public bool CustomText1_state { get; set; }
        public string? CustomText1_name { get; set; }
        public bool CustomText2_state { get; set; }
        public string? CustomText2_name { get; set; }
        public bool CustomText3_state { get; set; }
        public string? CustomText3_name { get; set; }
        public bool CustomInt1_state { get; set; }
        public string? CustomInt1_name { get; set; }
        public bool CustomInt2_state { get; set; }
        public string? CustomInt2_name { get; set; }
        public bool CustomInt3_state { get; set; }
        public string? CustomInt3_name { get; set; }
        public bool CustomBool1_state { get; set; }
        public string? CustomBool1_name { get; set; }
        public bool CustomBool2_state { get; set; }
        public string? CustomBool2_name { get; set; }
        public bool CustomBool3_state { get; set; }
        public string? CustomBool3_name { get; set; }
        [Required]
        public bool IsDeleted {  get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
