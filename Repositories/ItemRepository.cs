using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CourseProject_backend.Repositories
{
    public class ItemRepository : IRepository<Item>
    {
        private readonly CollectionDBContext _dbContext;

        public ItemRepository(CollectionDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Item item)
        {
            await _dbContext.Items.AddAsync(item);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Item item)
        {
            _dbContext.Items.Remove(item);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Item>> GetValue(Expression<Func<Item, bool>> predicat)
        {
            var item = (await _dbContext.Items
                         .Where(predicat)
                         .ToListAsync());

            return item;
        }

        public async Task SaveUpdates()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
