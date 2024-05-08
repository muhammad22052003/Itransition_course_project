using CourseProject_backend.Entities;

namespace CourseProject_backend.Entities.interfaces
{
    public interface IComment : IDBModel
    {
        public string Text { get; set; }

        public User User { get; set; }

        public Item Item { get; set; }
    }
}
