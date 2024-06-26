﻿using CourseProject_backend.CustomDbContext;
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
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Enums.Entities;

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

        public async Task DeleteRangeById(string[] collectionsId, User user)
        {
            IQueryable<MyCollection> query = _dbContext.Collections;

            query = query.Where((col) => collectionsId.Contains(col.Id) &&
            (user.IsAdmin() || col.User.Id == user.Id));

            var collections = await query.ToListAsync();

            _dbContext.RemoveRange(collections);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MyCollection>> GetValue(Expression<Func<MyCollection, bool>> predicat)
        {
            var collections = await _dbContext.Collections
                         .Where(predicat)
                         .Include(c => c.Items)
                         .Include(c => c.User)
                         .Include(c => c.Category)
                         .ToListAsync();

            return collections;
        }

        public async Task<IEnumerable<MyCollection>> GetCollectionList(CollectionDataFilter filter,
                                                                    string value,
                                                                    DataSort sort,
                                                                    int page,
                                                                    int pageSize,
                                                                    string categoryName)
        {
            page = page == 0 ? 1 : page;

            var sortedQuery = SortData(sort);
            List<MyCollection> collections = new List<MyCollection>();

            if(sortedQuery == null)
            {
                sortedQuery = _dbContext.Collections;
            }

            if (!categoryName.IsNullOrEmpty())
            {
                sortedQuery = sortedQuery.Where((c) => c.Category.Name.ToLower() == categoryName.ToLower());
            }

            switch (filter)
            {
                case CollectionDataFilter.byName:
                    {
                        if (!value.IsNullOrEmpty())
                        {
                            sortedQuery = sortedQuery
                                          .Where((c) => c.Name.ToLower() == value.ToLower());
                        }

                        collections = await sortedQuery
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(c => c.Items)
                            .Include(c => c.Category)
                            .Include(c => c.User)
                            .ToListAsync();
                    }
                    break;
                case CollectionDataFilter.byId:
                    {
                        collections = await sortedQuery
                            .Where((c) => c.Id.ToLower() == value.ToLower())
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(c => c.Items)
                            .Include(c => c.Category)
                            .Include(c => c.User)
                            .ToListAsync();
                    }
                    break;
                case CollectionDataFilter.byAuthorId:
                    {
                        collections = await sortedQuery
                            .Where((c) => c.User.Id.ToLower() == value.ToLower())
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(c => c.Items)
                            .Include(c => c.Category)
                            .Include(c => c.User)
                            .ToListAsync();
                    }
                    break;
                case CollectionDataFilter.byDefault:
                    {
                        collections = await sortedQuery
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(c => c.Items)
                            .Include(c => c.Category)
                            .Include(c => c.User)
                            .ToListAsync();
                    }
                    break;
            }

            return collections;
        }

        public async Task<int> GetCollectionsSize(CollectionDataFilter filter,
                                                                    string value,
                                                                    DataSort sort,
                                                                    string categoryName)
        {
            IQueryable<MyCollection> sortedQuery = _dbContext.Collections;

            if (!categoryName.IsNullOrEmpty())
            {
                sortedQuery = _dbContext.Collections.Where(c => c.Category.Name.ToLower() == categoryName.ToLower());
            }

            switch (filter)
            {
                case CollectionDataFilter.byName:
                    {
                         return await sortedQuery
                            .Where((c) => c.Name.ToLower() == value.ToLower())
                            .CountAsync();
                    }
                case CollectionDataFilter.byId:
                    {
                        return await sortedQuery
                            .Where((c) => c.Id.ToLower() == value.ToLower())
                            .CountAsync();
                    }
                case CollectionDataFilter.byAuthorId:
                    {
                        return await sortedQuery
                            .Where((c) => c.User.Id.ToLower() == value.ToLower())
                            .CountAsync();
                    }
                    break;
                case CollectionDataFilter.byDefault:
                    {
                        return await sortedQuery
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
                        return _dbContext.Collections.OrderByDescending((c) => c.CreatedTime);
                    }
                case DataSort.bySize:
                    {
                        return _dbContext.Collections.OrderByDescending((c) => c.Items.Count);
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
