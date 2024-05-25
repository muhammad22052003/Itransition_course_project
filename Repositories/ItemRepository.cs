using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Delegates;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.CustomDbContext;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Interfaces.Repositories;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task DeleteRangeById(string[] itemsId, string userId)
        {
            IQueryable<Item> query = _dbContext.Items;

            query = query.Where((it) => itemsId.Contains(it.Id) &&
            (it.Collection.User.Role == UserRoles.Admin.ToString() ||
            it.Collection.User.Id == userId));

            var items = await query.ToListAsync();

            _dbContext.RemoveRange(items);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Item>> GetValue(Expression<Func<Item, bool>> predicat)
        {
            var items = await _dbContext.Items
                         .Where(predicat)
                         .Include(i => i.Tags)
                         .Include(i => i.PositiveReact)
                         .Include(i => i.Comments).ThenInclude((x)=>x.User)
                         .Include(i => i.Views)
                         .ToListAsync();

            return items;
        }

        public async Task<IEnumerable<Item>> GetItemsList(ItemsDataFilter filter,
                                                      string value,
                                                      DataSort sort,
                                                      int page,
                                                      int pageSize,
                                                      string categoryName)
        {
            var sortedQuery = SortData(sort);
            List<Item> items = new List<Item>();

            if (sortedQuery == null)
            {
                sortedQuery = _dbContext.Items;
            }

            if (!categoryName.IsNullOrEmpty())
            {
                sortedQuery = sortedQuery.Where(it => it.Collection.Category.Name.ToLower() == categoryName.ToLower());
            }

            switch (filter)
            {
                case ItemsDataFilter.byDefault:
                    {
                        items = await sortedQuery
                            .Where((item) => true)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(i => i.Tags)
                            .Include(i => i.PositiveReact)
                            .Include(i => i.Comments)
                            .Include(i => i.Views)
                            .Include(i => i.Collection)
                            .ThenInclude(c => c.User)
                            .ToListAsync();
                    }
                    break;
                case ItemsDataFilter.byId:
                    {
                        items = await sortedQuery
                            .Where((item) => item.Id == value)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(i => i.Tags)
                            .Include(i => i.PositiveReact)
                            .Include(i => i.Comments)
                            .Include(i => i.Views)
                            .Include(i => i.Collection)
                            .ThenInclude(c => c.User)
                            .ToListAsync();
                    }
                    break;
                case ItemsDataFilter.byTag:
                    {
                        items = await sortedQuery
                            .Where((item) => item.Tags.Where(t => t.Name == value).FirstOrDefault() != null)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(i => i.Tags)
                            .Include(i => i.PositiveReact)
                            .Include(i => i.Comments)
                            .Include(i => i.Views)
                            .Include(i => i.Collection)
                            .ThenInclude(c => c.User)
                            .ToListAsync();
                    }
                    break;
                case ItemsDataFilter.byCollectionId:
                    {
                        items = await sortedQuery
                            .Where((item) => item.Collection.Id == value)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(i => i.Tags)
                            .Include(i => i.PositiveReact)
                            .Include(i => i.Comments)
                            .Include(i => i.Views)
                            .Include(i => i.Collection)
                            .ThenInclude(c => c.User)
                            .ToListAsync();
                    }
                    break;
                case ItemsDataFilter.bySearch:
                    {
                        items = await GetItemsBySearchTextQuery(sortedQuery, value, pageSize, page)
                            .Include(i => i.Tags)
                            .Include(i => i.PositiveReact)
                            .Include(i => i.Comments)
                            .Include(i => i.Views)
                            .Include(i => i.Collection)
                            .ThenInclude(c => c.User)
                            .ToListAsync();
                    }
                    break;
                default:
                    break;
            }

            return items;
        }

        public IQueryable<Item> GetItemsBySearchTextQuery(IQueryable<Item> sortedQuery,
                                                                  string searchText,
                                                                  int pageSize,
                                                                  int page)
        {
            if(searchText == null || searchText.Replace(" ", "").IsNullOrEmpty())
            {
                return sortedQuery
               .Skip((page - 1) * pageSize)
               .Take(pageSize);
            }

            return sortedQuery
                .Where((x) => x.SearchVector.Matches(searchText) ||
                       x.Comments.FirstOrDefault(c => c.SearchVector.Matches(searchText)) != null ||
                       x.Tags.FirstOrDefault(t => t.Name.ToLower() == searchText.ToLower()) != null)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            /*return sortedQuery
                .Where((x) => EF.Functions.ILike(x.Name.ToLower(), $"%{searchText.ToLower()}%")
                           || EF.Functions.ILike(x.CustomText1.ToLower(), $"%{searchText.ToLower()}%")
                           || EF.Functions.ILike(x.CustomText2.ToLower(), $"%{searchText.ToLower()}%")
                           || EF.Functions.ILike(x.CustomText3.ToLower(), $"%{searchText.ToLower()}%")
                           || EF.Functions.ILike(x.CustomString1.ToLower(), $"%{searchText.ToLower()}%")
                           || EF.Functions.ILike(x.CustomString2.ToLower(), $"%{searchText.ToLower()}%")
                           || EF.Functions.ILike(x.CustomString3.ToLower(), $"%{searchText.ToLower()}%")
                           || x.Comments.FirstOrDefault((c) => EF.Functions.ILike(c.Text.ToLower(), $"%{searchText.ToLower()}%")) != null
                           || x.Tags.FirstOrDefault((c) => EF.Functions.ILike(c.Name.ToLower(), $"%{searchText.ToLower()}%")) != null)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);*/
        }

        public async Task<int> GetItemsCount(ItemsDataFilter filter,
                                       string value,
                                       DataSort sort,
                                       string categoryName)
        {
            IQueryable<Item> sortedQuery = _dbContext.Items;

            if (!categoryName.IsNullOrEmpty())
            {
                sortedQuery = sortedQuery.Where(c => c.Collection.Category.Name.ToLower() == categoryName.ToLower());
            }

            switch (filter)
            {
                case ItemsDataFilter.byDefault:
                    {
                        return await sortedQuery
                            .Where((item) => true)
                            .CountAsync();
                    }
                case ItemsDataFilter.byId:
                    {
                        return await sortedQuery
                            .Where((item) => item.Id == value)
                            .CountAsync();
                    }
                case ItemsDataFilter.byTag:
                    {
                        return await sortedQuery
                            .Where((item) => item.Tags
                            .FirstOrDefault((x) => x.Name == value) != null)
                            .CountAsync();
                    }
                case ItemsDataFilter.byCollectionId:
                    {
                        return await sortedQuery
                            .Where((item) => item.Collection.Id == value)
                            .CountAsync();
                    }
                case ItemsDataFilter.bySearch:
                    {
                        //  EF.Functions.ILike delegate no supported by linq query
                        //LikeDelegate likeFunction = _dbContext.GetLikeDelegate();

                        return await sortedQuery
                            .Where((x) => EF.Functions.ILike(x.Name.ToLower(), $"%{value.ToLower()}%")
                                       || EF.Functions.ILike(x.CustomText1.ToLower(), $"%{value.ToLower()}%")
                                       || EF.Functions.ILike(x.CustomText2.ToLower(), $"%{value.ToLower()}%")
                                       || EF.Functions.ILike(x.CustomText3.ToLower(), $"%{value.ToLower()}%")
                                       || EF.Functions.ILike(x.CustomString1.ToLower(), $"%{value.ToLower()}%")
                                       || EF.Functions.ILike(x.CustomString2.ToLower(), $"%{value.ToLower()}%")
                                       || EF.Functions.ILike(x.CustomString3.ToLower(), $"%{value.ToLower()}%")
                                       || x.Comments
                                       .FirstOrDefault((c) => EF.Functions.ILike(c.Text, $"%{value}%")) != null
                                       || x.Tags.FirstOrDefault((c) => EF.Functions.ILike(c.Name, $"%{value.ToLower()}%")) != null)
                            .CountAsync();
                    }
                default:
                    break;
            }
            return 1;
        }

        public async Task SaveUpdates()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Item>? SortData(DataSort sort)
        {
            switch (sort)
            {
                case DataSort.byName:
                    {
                        return _dbContext.Items.OrderBy((c) => c.Name);
                    }
                case DataSort.byDate:
                    {
                        return _dbContext.Items.OrderByDescending((c) => c.CreatedTime);
                    }
                case DataSort.byLike:
                    {
                        return _dbContext.Items.OrderByDescending((c) => c.PositiveReact.Count);
                    }
                case DataSort.byView:
                    {
                        return _dbContext.Items.OrderByDescending((c) => c.Views.Count);
                    }
                case DataSort.bySize:
                    {
                        return null;
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
