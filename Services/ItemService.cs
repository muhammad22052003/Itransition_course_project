using CourseProject_backend.Builders;
using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Delegates;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.CustomDbContext;
using CourseProject_backend.Interfaces.Helpers;
using CourseProject_backend.Interfaces.Repositories;
using CourseProject_backend.Interfaces.Services;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySql.EntityFrameworkCore.Extensions;

namespace CourseProject_backend.Services
{
    public class ItemService : IModelService
    {
        private readonly ItemRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly CollectionDBContext _dbContext;
        public ItemService
        (
            ItemRepository repository,
            IConfiguration configuration,
            CollectionDBContext dbContext
        )
        {
            _dbContext = dbContext;
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<bool> CreateItem(ItemPostModel model)
        {
            ItemBuilder itemBuilder = new ItemBuilder(_configuration);

            MyCollection? collection = (await _dbContext.Collections
                .Where((x)=>x.Id == model.CollectionId)
                .ToListAsync()).FirstOrDefault();

            if( collection == null ) { return false; }

            itemBuilder.SetParameters(name: model.Name,
                                      collection: collection);

            Item item = itemBuilder.Build() as Item;

            await _repository.Add(item);

            return true;
        }

        public async Task<bool> UpdateItem(ItemPostModel model)
        {
            Item? item = (await _repository.GetValue((i) => i.Id.Equals(model.Id))).FirstOrDefault();

            if (item == null) { return false; }

            MyCollection? collection = (await _dbContext.Collections
                .Where((x) => x.Id == model.CollectionId)
                .ToListAsync()).FirstOrDefault();

            if (collection == null) { return false; }

            if (collection.CustomText1_state) { item.CustomText1 = model.CustomText1; }
            if (collection.CustomText2_state) { item.CustomText2 = model.CustomText2; }
            if (collection.CustomText3_state) { item.CustomText3 = model.CustomText3; }

            if (collection.CustomString1_state) { item.CustomString1 = model.CustomString1; }
            if (collection.CustomString2_state) { item.CustomString2 = model.CustomString2; }
            if (collection.CustomString3_state) { item.CustomString3 = model.CustomString3; }

            if (collection.CustomInt1_state) { item.CustomInt1 = model.CustomInt1; }
            if (collection.CustomInt2_state) { item.CustomInt2 = model.CustomInt2; }
            if (collection.CustomInt3_state) { item.CustomInt3 = model.CustomInt3; }

            if (collection.CustomBool1_state) { item.CustomBool1 = model.CustomBool1; }
            if (collection.CustomBool2_state) { item.CustomBool2 = model.CustomBool2; }
            if (collection.CustomBool3_state) { item.CustomBool3 = model.CustomBool3; }

            await SaveUpdates();

            return true;
        }

        public async Task<List<Item>> GetItems(ItemsDataFilter filter,
                                               string value)
        {
            List<Item> items = new List<Item>();

            switch (filter)
            {
                case ItemsDataFilter.byCollectionId:
                    {
                        items.AddRange(await _repository
                            .GetValue((it) => it.Collection.Id == value));
                    }break;
                case ItemsDataFilter.byTag:
                    {
                        items.AddRange(await _repository
                            .GetValue((item) => item.Tags.FirstOrDefault((x)=>x.Name == value) != null));
                    }break;
            }

            return items;
        }

        public List<Item> SortData(IEnumerable<Item> collections,
                                                        DataSort sort)
        {
            List<Item> newCollections = collections.ToList();

            switch (sort)
            {
                case DataSort.byDate:
                    {
                        newCollections.Sort((x, y) =>
                        {
                            if (x.CreatedTime < y.CreatedTime) { return -1; }
                            if (x.CreatedTime > y.CreatedTime) { return 1; }
                            return 0;
                        });
                    }
                    break;
                case DataSort.byName:
                    {
                        newCollections.Sort((x, y) =>
                        {
                            if (x.Name[0] < y.Name[0]) { return -1; }
                            if (x.Name[0] > y.Name[0]) { return 1; }
                            return 0;
                        });
                    }
                    break;
            }

            return newCollections;
        }

        public List<Item> GetForPage(List<Item> collections, int page)
        {
            return collections.Slice((page - 1) * 10, (page - 1) * 10 + 10);
        }

        public async Task<List<Item>> FullTextSearch(string text)
        {
            List<Item> items = new List<Item>();

            LikeDelegate likeFunction;

            switch (_dbContext.CurrentDbSystem)
            {
                case DBSystem.MYSQL:
                    likeFunction = EF.Functions.Like;
                    break;
                case DBSystem.POSTGRES:
                    {
                        likeFunction = EF.Functions.ILike;
                    }
                    break;
                default:
                    throw new NotImplementedException("Undefined DbSytem");
            }

            items.AddRange(await _dbContext.Items
                            .Where((x) => likeFunction(x.Name, $"%{text}%")
                                       || likeFunction(x.CustomText1, $"%{text}%")
                                       || likeFunction(x.CustomText2, $"%{text}%")
                                       || likeFunction(x.CustomText3, $"%{text}%")
                                       || likeFunction(x.CustomString1, $"%{text}%")
                                       || likeFunction(x.CustomString2, $"%{text}%")
                                       || likeFunction(x.CustomString3, $"%{text}%"))
                            .ToListAsync());

            items.AddRange((await _dbContext.Commentaries
                            .Where((x) => likeFunction(x.Text, $"%{text}%")).ToListAsync())
                            .Select((x) => x.Item));

            items = items.DistinctBy((x) => x.Id).ToList();

            return items;
        }

        public async Task SaveUpdates()
        {
            await _repository.SaveUpdates();
        }
    }
}
