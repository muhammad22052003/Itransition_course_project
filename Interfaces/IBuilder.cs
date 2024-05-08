using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CourseProject_backend.Interfaces
{
    public interface IBuilder
    {
        public IDBModel Build();
    }
}
