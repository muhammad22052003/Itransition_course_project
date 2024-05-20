using CourseProject_backend.CustomDbContext;

namespace CourseProject_backend.Interfaces.Services
{
    public interface IModelService
    {
        public Task SaveUpdates();

        public void Initialize(CollectionDBContext dBContext);
    }
}
