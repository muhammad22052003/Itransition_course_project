using CourseProject_backend.Entities.interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_backend.Entities
{
    [Index(nameof(CustomString1))]
    [Index(nameof(CustomString2))]
    [Index(nameof(CustomString3))]
    [Index(nameof(CustomText1))]
    [Index(nameof(CustomText2))]
    [Index(nameof(CustomText3))]
    [Index(nameof(CustomInt1))]
    [Index(nameof(CustomInt2))]
    [Index(nameof(CustomInt3))]
    [Index(nameof(CustomBool1))]
    [Index(nameof(CustomBool2))]
    [Index(nameof(CustomBool3))]
    [Index(nameof(CreatedTime))]
    public class Item : IItem
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedTime { get; set; }
        public MyCollection Collection { get; set; }
        public ICollection<PositiveReaction> PositiveReact { get; set; }
        public ICollection<NegativeReaction> NegativeReact { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public string CustomString1 { get; set; }
        public string CustomString2 { get; set; }
        public string CustomString3 { get; set; }
        public string CustomText1 { get; set; }
        public string CustomText2 { get; set; }
        public string CustomText3 { get; set; }
        public int CustomInt1 { get; set; }
        public int CustomInt2 { get; set; }
        public int CustomInt3 { get; set; }
        public bool CustomBool1 { get; set; }
        public bool CustomBool2 { get; set; }
        public bool CustomBool3 { get; set; }
        public bool IsDeleted { get; set; }
    }
}
