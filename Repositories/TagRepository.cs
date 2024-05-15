using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

            foreach (var tag in tags)
            {
                Tag? newTag = (await _dBContext.Tags.Where((t)=>t.Name == tag)
                    .ToListAsync())
                    .FirstOrDefault();

                if(newTag == null)
                {
                    newTag = new Tag(tag, item);
                }

                resultTags.Add(newTag);

                _dBContext.Add(newTag);
            }

            await _dBContext.SaveChangesAsync();

            return resultTags;
        }

        public async Task Add(Tag tag)
        {
            _dBContext.Add(tag);

            await _dBContext.SaveChangesAsync();
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
