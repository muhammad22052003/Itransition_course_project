using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseProject_backend.Interfaces.Repositories
{
    public interface IRepository<TModel> where TModel : IDBModel
    {
        public Task Add(TModel item);

        public Task<IEnumerable<TModel>> GetValue(Expression<Func<TModel, bool>> predicat);

        public Task Delete(TModel item);

        public Task SaveUpdates();
    }
}
