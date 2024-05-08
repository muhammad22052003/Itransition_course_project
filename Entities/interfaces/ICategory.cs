using Microsoft.EntityFrameworkCore;

namespace CourseProject_backend.Entities.interfaces
{
    public interface ICategory : IDBModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }
    }
}
