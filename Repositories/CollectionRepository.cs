using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Enums;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CourseProject_backend.Delegates;
using CourseProject_backend.Enums.CustomDbContext;
using MySql.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            var collections = (await _dbContext.Collections
                         .Where(predicat)
                         .ToListAsync());

            return collections;
        }

        public async Task<IEnumerable<MyCollection>> GetCollectionList(CollectionDataFilter filter,
                                                                    string value,
                                                                    DataSort sort,
                                                                    int page,
                                                                    int pageSize)
        {

            var sortedQuery = SortData(sort);
            List<MyCollection> collections = new List<MyCollection>();

            if(sortedQuery == null)
            {
                sortedQuery = _dbContext.Collections.Where((c) => true);
            }

            switch (filter)
            {
                case CollectionDataFilter.byName:
                    {
                        //  EF.Functions.ILike delegate no supported by linq query
                        //LikeDelegate likeFunction = _dbContext.GetLikeDelegate();

                        collections = await sortedQuery
                            .Where((c) => EF.Functions.ILike(c.Name.ToLower(), $"%{value.ToLower()}%"))
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize).ToListAsync();
                    }
                    break;
                case CollectionDataFilter.byCategory:
                    {
                        collections = await sortedQuery
                            .Where((c) => c.Category.Name.ToLower() == value.ToLower())
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                    }
                    break;
                case CollectionDataFilter.byId:
                    {
                        collections = await sortedQuery
                            .Where((c) => c.Id == value)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize).ToListAsync();
                    }
                    break;
                case CollectionDataFilter.byAuthorId:
                    {
                        collections = await sortedQuery
                            .Where((c) => c.User.Id == value)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                    }
                    break;
                case CollectionDataFilter.byDefault:
                    {
                        collections = await sortedQuery
                            .Where((c) => true)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                    }
                    break;
            }

            return collections;
        }

        public async Task<int> GetCollectionsSize(CollectionDataFilter filter,
                                                                    string value,
                                                                    DataSort sort)
        {
            switch (filter)
            {
                case CollectionDataFilter.byName:
                    {
                         return await _dbContext.Collections
                            .Where((c) => EF.Functions.ILike(c.Name.ToLower(), $"%{value.ToLower()}%"))
                            .CountAsync();
                    }
                case CollectionDataFilter.byCategory:
                    {
                        return await _dbContext.Collections
                            .Where((c) => c.Category.Name.ToLower() == value.ToLower())
                            .CountAsync();
                    }
                case CollectionDataFilter.byId:
                    {
                        return await _dbContext.Collections
                            .Where((c) => c.Id == value)
                            .CountAsync();
                    }
                case CollectionDataFilter.byAuthorId:
                    {
                        return await _dbContext.Collections
                            .Where((c) => c.User.Id == value)
                            .CountAsync();
                    }
                    break;
                case CollectionDataFilter.byDefault:
                    {
                        return await _dbContext.Collections
                            .Where((c) => true)
                            .CountAsync();
                    }
            }

            return 1;
        }

        public async Task SaveUpdates()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<MyCollection>? SortData(DataSort sort)
        {
            switch (sort)
            {
                case DataSort.byName:
                    {
                        return _dbContext.Collections.OrderBy((c)=>c.Name);
                    }
                case DataSort.byDate:
                    {
                        return _dbContext.Collections.OrderBy((c) => c.CreatedTime);
                    }
                case DataSort.bySize:
                    {
                        return _dbContext.Collections.OrderBy((c) => c.Items.Count);
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
