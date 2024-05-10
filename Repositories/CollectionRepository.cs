using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseProject_backend.Repositories
{
    public class CollectionRepository : IRepository<MyCollection>
    {
        private readonly CollectionDBContext _dbContext;

        public CollectionRepository(CollectionDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(MyCollection item)
        {
            await _dbContext.Collections.AddAsync(item);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(MyCollection item)
        {
            _dbContext.Collections.Remove(item);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MyCollection>> GetValue(Expression<Func<MyCollection, bool>> predicat)
        {
            var collection = (await _dbContext.Collections
                         .Where(predicat)
                         .ToListAsync());

            return collection;
        }

        public async Task SaveUpdates()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
