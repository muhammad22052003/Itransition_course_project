using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Interfaces.Entities;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseProject_backend.Repositories
{
    public class ReactionRepository : IRepository<PositiveReaction>
    {
        private readonly CollectionDBContext _dbContext;

        public ReactionRepository
        (
            CollectionDBContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task Add(PositiveReaction item)
        {
            _dbContext.Add(item);
        }

        public async Task<IEnumerable<PositiveReaction>> GetValue(Expression<Func<PositiveReaction, bool>> predicate)
        {
            return await _dbContext.PositiveReactions
                .Where(predicate)
                .ToArrayAsync();
        }

        public async Task Delete(PositiveReaction item)
        {
            _dbContext.Remove(item);
        }

        public async Task SaveUpdates()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<PositiveReaction>? SortData(DataSort sort)
        {
            throw new NotImplementedException();
        }
    }
}
