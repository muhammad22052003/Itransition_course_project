using CourseProject_backend.Builders;
using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Interfaces.Helpers;
using CourseProject_backend.Interfaces.Repositories;
using CourseProject_backend.Interfaces.Services;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_backend.Services
{
    public class CollectionService : IModelService
    {
        private readonly CollectionRepository _repository;
        private readonly CollectionDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly CategoryRepository _categoryRepository;

        public CollectionService
        (
            CollectionRepository repository,
            IConfiguration configuration,
            CollectionDBContext dbContext,
            UserService userService,
            CategoryRepository categoryRepository
        )
        {
            _dbContext = dbContext;
            _repository = repository;
            _configuration = configuration;
            _userService = userService;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<MyCollection>> GetCollectionList(CollectionDataFilter filter,
                                                                    string value,
                                                                    DataSort sort,
                                                                    int page,
                                                                    int pageSize,
                                                                    string categoryName)
        {
            var collections = await _repository.GetCollectionList
                              (filter, value, sort, page, pageSize, categoryName);

            return collections;
        }

        public async Task<int> GetCollectionsCount(CollectionDataFilter filter,
                                                                    string value,
                                                                    DataSort sort,
                                                                    string categoryName)
        {
            return await _repository.GetCollectionsSize(filter, value, sort, categoryName);
        }

        public async Task<bool> CreateCollection(CollectionCreateModel model, User authorUser)
        {
            CollectionBuilder collectionBuilder = new CollectionBuilder(_configuration);

            Category? category = (await _categoryRepository
                .GetValue(c=>c.Name.ToLower() == model.CategoryName.ToLower())).FirstOrDefault();

            if (authorUser == null || category == null)
            {
                return false;
            }

            collectionBuilder.SetParameters(name: model.Name,
                                            description: model.Description,
                                            user: authorUser,
                                            category: category
                                            );

            MyCollection collection = collectionBuilder.Build() as MyCollection;

            #region coolection inistializing

            collection.Name = model.Name;
            collection.Description = model.Description;

            collection.CustomText1_state = model.CustomText1_name != null;
            collection.CustomText1_name = model.CustomText1_name;
            collection.CustomText2_state = model.CustomText2_name != null;
            collection.CustomText2_name = model.CustomText2_name;
            collection.CustomText3_state = model.CustomText3_name != null;
            collection.CustomText3_name = model.CustomText3_name;

            collection.CustomString1_state = model.CustomString1_name != null;
            collection.CustomString1_name = model.CustomString1_name;
            collection.CustomString2_state = model.CustomString2_name != null;
            collection.CustomString2_name = model.CustomString2_name;
            collection.CustomString3_state = model.CustomString3_name != null;
            collection.CustomString3_name = model.CustomString3_name;

            collection.CustomInt1_state = model.CustomInt1_name != null;
            collection.CustomInt1_name = model.CustomInt1_name;
            collection.CustomInt2_state = model.CustomInt2_name != null;
            collection.CustomInt2_name = model.CustomInt2_name;
            collection.CustomInt3_state = model.CustomInt3_name != null;
            collection.CustomInt3_name = model.CustomInt3_name;

            collection.CustomBool1_state = model.CustomBool1_name != null;
            collection.CustomBool1_name = model.CustomBool1_name;
            collection.CustomBool2_state = model.CustomBool2_name != null;
            collection.CustomBool2_name = model.CustomBool2_name;
            collection.CustomBool3_state = model.CustomBool3_name != null;
            collection.CustomBool3_name = model.CustomBool3_name;

            collection.CustomDate1_state = model.CustomDate1_name != null;
            collection.CustomDate1_name = model.CustomDate1_name;
            collection.CustomDate2_state = model.CustomDate2_name != null;
            collection.CustomDate2_name = model.CustomDate2_name;
            collection.CustomDate3_state = model.CustomDate3_name != null;
            collection.CustomDate3_name = model.CustomDate3_name;
            #endregion

            await _repository.Add(collection);
            return true;
        }

        public async Task<MyCollection?> GetById(string id = null)
        {
            if (id == null) { return null; }

            return (await _repository.GetValue((x)=>x.Id == id)).FirstOrDefault();
        }

        public async Task<MyCollection?> GetByItemId(string id = null)
        {
            if (id == null) { return null; }

            return (await _repository.GetValue((x) => x.Items.FirstOrDefault((i)=>i.Id == id) != null)).FirstOrDefault();
        }

        public async Task DeleteRange(string[] collectionsId, string userId)
        {
            await _repository.DeleteRangeById(collectionsId, userId);
        }
        public async Task SaveUpdates()
        {
            await _repository.SaveUpdates();
        }
    }
}
