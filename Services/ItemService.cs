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
        private readonly TagRepository _tagRepository;
        private readonly IConfiguration _configuration;
        private readonly CollectionDBContext _dbContext;
        public ItemService
        (
            ItemRepository repository,
            IConfiguration configuration,
            CollectionDBContext dbContext,
            TagRepository tagRepository
        )
        {
            _dbContext = dbContext;
            _repository = repository;
            _configuration = configuration;
            _tagRepository = tagRepository;
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

            if (collection == null) { return false; }

            string[]? tagsStrings = model?.Tags?.Split(' ');

            if(tagsStrings == null) { return false; }

            List<Tag> tags = (await _tagRepository.GetAndCreate(tagsStrings, item)).ToList();

            item.Tags = tags;

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

            if (collection.CustomDate1_state) { item.CustomDate1 = model.CustomDate1?.ToUniversalTime(); }
            if (collection.CustomDate2_state) { item.CustomDate2 = model.CustomDate2?.ToUniversalTime(); }
            if (collection.CustomDate3_state) { item.CustomDate3 = model.CustomDate3?.ToUniversalTime(); }

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

            if (collection.CustomDate1_state) { item.CustomDate1 = model.CustomDate1?.ToUniversalTime(); }
            if (collection.CustomDate2_state) { item.CustomDate2 = model.CustomDate2?.ToUniversalTime(); }
            if (collection.CustomDate3_state) { item.CustomDate3 = model.CustomDate3?.ToUniversalTime(); }

            await SaveUpdates();

            return true;
        }

        public async Task<List<Item>> GetItemsList(ItemsDataFilter filter,
                                               string value,
                                               DataSort sort,
                                               int page,
                                               int pageSize)
        {
            List<Item> items = (await _repository
                .GetItemsList(filter, value, sort, page, pageSize))
                .ToList();

            return items;
        }

        public async Task<int> GetItemsCount(ItemsDataFilter filter,
                                               string value,
                                               DataSort sort)
        {
            int count = (await _repository
                .GetItemsCount(filter, value, sort));

            return count;
        }

        public async Task SaveUpdates()
        {
            await _repository.SaveUpdates();
        }
    }
}
