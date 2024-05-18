using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseProject_backend.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly CollectionDBContext _dBContext;

        public CategoryRepository(CollectionDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task Add(Category item)
        {
            _dBContext.Categories.Add(item);

            await SaveUpdates();
        }

        public async Task<IEnumerable<Category>> GetValue(Expression<Func<Category, bool>> predicat)
        {
            List<Category> category = await _dBContext.Categories
                                       .Where(predicat).ToListAsync();

            return category;
        }

        public async Task Delete(Category item)
        {
            _dBContext.Categories.Remove(item);

            await SaveUpdates();
        }

        public async Task SaveUpdates()
        {
            await _dBContext.SaveChangesAsync();
        }

        public IQueryable<Category>? SortData(DataSort sort)
        {
            throw new NotImplementedException();
        }
    }
}
