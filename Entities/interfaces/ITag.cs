using CourseProject_backend.Entities;

namespace CourseProject_backend.Entities.interfaces
{
    public interface ITag : IDBModel
    {
        string Name { set; get; }

        public Item Item { set; get; }
    }
}
