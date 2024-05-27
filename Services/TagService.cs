using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Interfaces.Services;
using CourseProject_backend.Repositories;
using Newtonsoft.Json.Linq;

namespace CourseProject_backend.Services
{
    public class TagService : IModelService
    {
        private TagRepository _tagRepository;

        public TagService()
        {
        }

        public void Initialize(CollectionDBContext dBContext)
        {
            _tagRepository = new TagRepository(dBContext);
        }

        public async Task<IEnumerable<Tag>> GetTagsList(TagsDataFilter filter,
                                 string value,
                                 int page = 1,
                                 int pageSize = int.MaxValue)
        {
            return await _tagRepository.GetTagsList(filter, value, page, pageSize);
        }

        public async Task SaveUpdates()
        {
            await _tagRepository.SaveUpdates();
        }
    }
}
