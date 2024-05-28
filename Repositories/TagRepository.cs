using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CourseProject_backend.Repositories
{
    public class TagRepository : IRepository<Tag>
    {
        private readonly CollectionDBContext _dBContext;

        public TagRepository(CollectionDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<IEnumerable<Tag>> GetAndCreate(string[] tags, Item item)
        {
            List<Tag> resultTags = new List<Tag>();

            tags = tags.Distinct().ToArray();

            foreach (var tag in tags)

            {
                Tag newTag = new Tag(tag.ToLower(), item);

                resultTags.Add(newTag);
            }

            return resultTags;
        }

        public async Task Add(Tag tag)
        {
            _dBContext.Add(tag);

            await _dBContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetValue(Expression<Func<Tag, bool>> predicate)
        {
            return await _dBContext.Tags
                .Where(predicate)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsList(TagsDataFilter filter,
                                                        string value,
                                                        int page = 1,
                                                        int pageSize = int.MaxValue)
        {
            page = page == 0 ? 1 : page;

            List<Tag> tags = new List<Tag>();

            switch (filter)
            {
                case TagsDataFilter.byDefault:
                    {
                        tags = await _dBContext.Tags
                            .ToListAsync();
                    }
                    break;
                case TagsDataFilter.byPopular:
                    {
                        var items = await _dBContext.Items
                            .Include(x => x.Tags)
                            .ToListAsync();

                        tags = (await _dBContext.Tags
                            .GroupBy(x => x.Name)
                            .Select(x => x.First())
                            .Take(500)
                            .ToListAsync())
                                .OrderByDescending(x => items
                                    .Where(i => i.Tags.FirstOrDefault(y => y.Name == x.Name) != null)
                                    .Count())
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();


                    }
                    break;
                case TagsDataFilter.byItemId:
                    {
                        tags = await _dBContext.Tags
                            .Where(x => x.Item.Id.ToLower() == value.ToLower())
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                    }
                    break;
            }

            return tags;
        }

        public async Task Delete(Tag tag)
        {
            _dBContext.Remove(tag);

            await _dBContext.SaveChangesAsync();
        }

        public async Task SaveUpdates()
        {
            await _dBContext.SaveChangesAsync();
        }

        public IQueryable<Tag>? SortData(DataSort sort)
        {
            throw new NotImplementedException();
        }
    }
}
