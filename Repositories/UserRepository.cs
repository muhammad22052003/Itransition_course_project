using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace CourseProject_backend.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly CollectionDBContext _dbContext;

        public UserRepository
        (
            CollectionDBContext dbContext
        )
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
            var user = await _dbContext.Users
                         .Where(predicat)
                         .Include((u) => u.Collections)
                         .ThenInclude(c => c.Items)
                         .ToListAsync();

            return user;
        }

        public async Task Delete(User user)
        {
            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsersList(UsersDataFilter filter,
                                                          string value,
                                                          DataSort sort,
                                                          int page,
                                                          int pageSize)
        {
            var sortedQuery = SortData(sort);
            List<User> users = new List<User>();

            if (sortedQuery == null)
            {
                sortedQuery = _dbContext.Users;
            }

            switch (filter)
            {
                case UsersDataFilter.byName:
                    {

                        if (!value.IsNullOrEmpty())
                        {
                            sortedQuery = sortedQuery
                                         .Where((c) => c.Name.ToLower() == value.ToLower());
                        }

                        users = await sortedQuery
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include((u) => u.Collections)
                            .ThenInclude(c => c.Items)
                            .ToListAsync();
                    }
                    break;
                case UsersDataFilter.byStatus:
                    {
                        users = await sortedQuery
                            .Where((c) => c.Role.ToLower() == value.ToLower())
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include((u) => u.Collections)
                            .ThenInclude(c => c.Items)
                            .ToListAsync();
                    }
                    break;
                case UsersDataFilter.byId:
                    {
                        users = await sortedQuery
                            .Where((c) => c.Id.ToLower() == value.ToLower())
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include((u) => u.Collections)
                            .ThenInclude(c => c.Items)
                            .ToListAsync();
                    }
                    break;
                case UsersDataFilter.byEmail:
                    {
                        users = await sortedQuery
                            .Where((c) => c.Email.ToLower() == value.ToLower())
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include((u) => u.Collections)
                            .ThenInclude(c => c.Items)
                            .ToListAsync();
                    }
                    break;
                case UsersDataFilter.byDefault:
                    {
                        users = await sortedQuery
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include((u) => u.Collections)
                            .ThenInclude(c => c.Items)
                            .ToListAsync();
                    }
                    break;
            }

            return users;
        }

        public async Task<int> GetUsersCount(UsersDataFilter filter,
                                             string value,
                                             DataSort sort)
        {
            var sortedQuery = SortData(sort);
            List<User> users = new List<User>();

            if (sortedQuery == null)
            {
                sortedQuery = _dbContext.Users.Where((c) => true);
            }

            switch (filter)
            {
                case UsersDataFilter.byName:
                    {
                        return await sortedQuery
                            .Where((c) => c.Name.ToLower() == value.ToLower())
                            .CountAsync();
                    }
                case UsersDataFilter.byStatus:
                    {
                        return await sortedQuery
                            .Where((c) => c.Role.ToLower() == value.ToLower())
                            .CountAsync();
                    }
                case UsersDataFilter.byId:
                    {
                        return await sortedQuery
                            .Where((c) => c.Id.ToLower() == value.ToLower())
                            .CountAsync();
                    }
                case UsersDataFilter.byDefault:
                    {
                        return await sortedQuery.CountAsync();
                    }
            }

            return 1;
        }

        public async Task SaveUpdates()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task DemoteUsers(string[] userId)
        {
            var users = await _dbContext.Users
                .Where(x => userId.Contains(x.Id))
                .ToListAsync();

            foreach (var user in users)
            {
                if (!user.IsUser())
                {
                    user.Role = UserRoles.User.ToString();
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task PromoteUsers(string[] userId)
        {
            var users = await _dbContext.Users
                .Where(x => userId.Contains(x.Id))
                .ToListAsync();

            foreach (var user in users)
            {
                if (!user.IsAdmin())
                {
                    user.Role = UserRoles.Admin.ToString();
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUsers(string[] userId)
        {
            var users = await _dbContext.Users
                .Where(x => userId.Contains(x.Id))
                .Include(x => x.Collections)
                .ThenInclude(x => x.Items)
                .Include(x => x.PositiveReactions)
                .Include(x => x.PositiveReactions)
                .Include(x => x.Views)
                .ToListAsync();

            foreach (var user in users)
            {
                _dbContext.Users.Remove(user);
            }

            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<User>? SortData(DataSort sort)
        {
            switch (sort)
            {
                case DataSort.byName:
                    {
                        return _dbContext.Users.OrderBy((c) => c.Name);
                    }
                case DataSort.byDate:
                    {
                        return _dbContext.Users.OrderBy((c) => c.RegistrationTime);
                    }
                case DataSort.byDefault:
                    {
                        return null;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
