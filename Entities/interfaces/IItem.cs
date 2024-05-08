using CourseProject_backend.Entities;

namespace CourseProject_backend.Entities.interfaces
{
    public interface IItem : IDBModel
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedTime { get; set; }

        public ICollection<PositiveReaction> PositiveReact { get; set; }
        public ICollection<NegativeReaction> NegativeReact { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public MyCollection Collection { get; set; }

        public string CustomString1 { get; set; }
        public string CustomString2 { get; set; }
        public string CustomString3 { get; set; }
        // text
        public string CustomText1 { get; set; }
        public string CustomText2 { get; set; }
        public string CustomText3 { get; set; }
        // int
        public int CustomInt1 { get; set; }
        public int CustomInt2 { get; set; }
        public int CustomInt3 { get; set; }
        // bool
        public bool CustomBool1 { get; set; }
        public bool CustomBool2 { get; set; }
        public bool CustomBool3 { get; set; }

        public bool IsDeleted { get; set; }
    }
}
