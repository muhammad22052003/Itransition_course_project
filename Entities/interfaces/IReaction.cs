using CourseProject_backend.Entities;

namespace CourseProject_backend.Entities.interfaces
{
    public interface IReaction : IDBModel
    {
        public string Name { get; set; }

        public string IconUrl { get; set; }

        public User User { get; set; }

        public Item Item { get; set; }
    }
}
