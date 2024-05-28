using CourseProject_backend.Builders;
using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Helpers;
using CourseProject_backend.Interfaces.Helpers;
using CourseProject_backend.Interfaces.Repositories;
using CourseProject_backend.Interfaces.Services;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_backend.Services
{
    public class CollectionService : IModelService
    {
        private CollectionRepository _collectionRepository;
        private UserRepository _userRepository;
        private CategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        private readonly CSVHepler _csvHepler;
        private readonly CollectionAdapter _collectionAdapter;

        public CollectionService
        (
            IConfiguration configuration,
            CSVHepler csvHepler,
            CollectionAdapter collectionAdapter
        )
        {
            
            _configuration = configuration;
            _csvHepler = csvHepler;
            _collectionAdapter = collectionAdapter;
        }

        public void Initialize(CollectionDBContext dBContext)
        {
            _collectionRepository = new CollectionRepository(dBContext);
            _userRepository = new UserRepository(dBContext);
            _categoryRepository = new CategoryRepository(dBContext);
        }

        public async Task<IEnumerable<MyCollection>> GetCollectionList(CollectionDataFilter filter,
                                                                    string value,
                                                                    DataSort sort,
                                                                    int page,
                                                                    int pageSize,
                                                                    string categoryName)
        {
            var collections = await _collectionRepository.GetCollectionList
                              (filter, value, sort, page, pageSize, categoryName);

            return collections;
        }

        public async Task<int> GetCollectionsCount(CollectionDataFilter filter,
                                                                    string value,
                                                                    DataSort sort,
                                                                    string categoryName)
        {
            return await _collectionRepository.GetCollectionsSize(filter, value, sort, categoryName);
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

            await _collectionRepository.Add(collection);
            return true;
        }

        public async Task<bool> UpdateCollection([FromBody] CollectionCreateModel model)
        {
            MyCollection? collection = await GetById(model.CollectionId);

            if (collection == null)
                return false;

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

            DefineCollectionItems(collection);

            await SaveUpdates();

            return true;
        }

        public async void DefineCollectionItems(MyCollection collection)
        {
            Item[] items = collection.Items.ToArray();

            for (int i = 0; i < collection.Items.Count; i++)
            {
                if (collection.CustomString1_state)
                    items[i].CustomString1 = items[i].CustomString1 == null ? string.Empty : items[i].CustomString1;

                if (collection.CustomString2_state)
                    items[i].CustomString2 = items[i].CustomString2 == null ? string.Empty : items[i].CustomString2;

                if (collection.CustomString3_state)
                    items[i].CustomString3 = items[i].CustomString3 == null ? string.Empty : items[i].CustomString3;
                ////////////
                if (collection.CustomText1_state)
                    items[i].CustomText1 = items[i].CustomText1 == null ? string.Empty : items[i].CustomText1;

                if (collection.CustomText2_state)
                    items[i].CustomText2 = items[i].CustomText2 == null ? string.Empty : items[i].CustomText2;

                if (collection.CustomText3_state)
                    items[i].CustomText3 = items[i].CustomText3 == null ? string.Empty : items[i].CustomText3;
                ////////////
                if (collection.CustomInt1_state)
                    items[i].CustomInt1 = items[i].CustomInt1 == null ? 0 : items[i].CustomInt1;

                if (collection.CustomInt2_state)
                    items[i].CustomInt2 = items[i].CustomInt2 == null ? 0 : items[i].CustomInt2;

                if (collection.CustomInt3_state)
                    items[i].CustomInt3 = items[i].CustomInt3 == null ? 0 : items[i].CustomInt3;
                ////////////
                if (collection.CustomBool1_state)
                    items[i].CustomBool1 = items[i].CustomBool1 == null ? false : items[i].CustomBool1;

                if (collection.CustomBool2_state)
                    items[i].CustomBool2 = items[i].CustomBool2 == null ? false : items[i].CustomBool2;

                if (collection.CustomBool3_state)
                    items[i].CustomBool3 = items[i].CustomBool3 == null ? false : items[i].CustomBool3;
                ////////////
                if (collection.CustomDate1_state)
                    items[i].CustomDate1 = items[i].CustomDate1 == null ? DateTime.MinValue.ToUniversalTime() : items[i].CustomDate1;

                if (collection.CustomDate2_state)
                    items[i].CustomDate2 = items[i].CustomDate2 == null ? DateTime.MinValue.ToUniversalTime() : items[i].CustomDate2;

                if (collection.CustomDate3_state)
                    items[i].CustomDate3 = items[i].CustomDate3 == null ? DateTime.MinValue.ToUniversalTime() : items[i].CustomDate3;
            }

            collection.Items = items;

        }

        public byte[] GetCollectionCsv(MyCollection collection)
        {
            var dataTable = _collectionAdapter.AdapteToListTable(collection);

            byte[] bytes = _csvHepler.GetStream(dataTable);

            return bytes;
        }
        public async Task<MyCollection?> GetById(string id = null)
        {
            if (id == null) { return null; }

            return (await _collectionRepository.GetValue((x)=>x.Id == id)).FirstOrDefault();
        }

        public async Task<MyCollection?> GetByItemId(string id = null)
        {
            if (id == null) { return null; }

            return (await _collectionRepository.GetValue((x) => x.Items.FirstOrDefault((i)=>i.Id == id) != null)).FirstOrDefault();
        }

        public async Task DeleteRange(string[] collectionsId, User user)
        {
            await _collectionRepository.DeleteRangeById(collectionsId, user);
        }
        public async Task SaveUpdates()
        {
            await _collectionRepository.SaveUpdates();
        }
    }
}
