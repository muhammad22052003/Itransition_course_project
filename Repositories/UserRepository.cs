using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace CourseProject_backend.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly CollectionDBContext _dbContext;

        public UserRepository(CollectionDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetValue(Expression<Func<User,bool>> predicat)
        {
            var user = (await _dbContext.Users
                         .Where(predicat)
                         .ToListAsync());

            return user;
        }

        public async Task Delete(User user)
        {
            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveUpdates()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
