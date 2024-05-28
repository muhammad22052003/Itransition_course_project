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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySql.EntityFrameworkCore.Extensions;

namespace CourseProject_backend.Services
{
    public class ItemService : IModelService
    {
        private ItemRepository _itemRepository;
        private TagRepository _tagRepository;
        private CollectionRepository _collectionRepository;
        private CommentRepository _commentRepository;
        private readonly IConfiguration _configuration;

        public ItemService(IConfiguration configuration)
        {
            
            _configuration = configuration;
        }

        public void Initialize(CollectionDBContext dBContext)
        {
            _itemRepository = new ItemRepository(dBContext);
            _tagRepository = new TagRepository(dBContext);
            _commentRepository = new CommentRepository(dBContext);
            _collectionRepository = new CollectionRepository(dBContext);
        }

        public async Task<bool> CreateItem(ItemPostModel model)
        {
            ItemBuilder itemBuilder = new ItemBuilder(_configuration);

            MyCollection? collection = (await _collectionRepository
                .GetValue(x => x.Id == model.CollectionId))
                .FirstOrDefault();

            if( collection == null ) { return false; }

            itemBuilder.SetParameters(name: model.Name,
                                      collection: collection);

            Item item = itemBuilder.Build() as Item;

            if (collection == null) { return false; }

            string[]? tagsStrings = model?.Tags?.Split(' ');

            if(tagsStrings == null) { return false; }

            tagsStrings = tagsStrings.Where(x => x.Replace(" ", "") != "").ToArray();

            List<Tag> tags = (await _tagRepository.GetAndCreate(tagsStrings, item)).ToList();

            item.Tags = tags;

            item.CustomText1 = collection.CustomText1_state ? model.CustomText1 : null;
            item.CustomText2 = collection.CustomText2_state ? model.CustomText2 : null;
            item.CustomText3 = collection.CustomText3_state ? model.CustomText3 : null;

            item.CustomString1 = collection.CustomString1_state ? model.CustomString1 : null;
            item.CustomString2 = collection.CustomString2_state ? model.CustomString2 : null;
            item.CustomString3 = collection.CustomString3_state ? model.CustomString3 : null;

            item.CustomInt1 = collection.CustomInt1_state ? model.CustomInt1 : null;
            item.CustomInt2 = collection.CustomInt2_state ? model.CustomInt2 : null;
            item.CustomInt3 = collection.CustomInt3_state ? model.CustomInt3 : null;

            item.CustomBool1 = collection.CustomBool1_state ? model.CustomBool1 : null;
            item.CustomBool2 = collection.CustomBool2_state ? model.CustomBool2 : null;
            item.CustomBool3 = collection.CustomBool3_state ? model.CustomBool3 : null;

            item.CustomDate1 = collection.CustomDate1_state ? model.CustomDate1?.ToUniversalTime() : null;
            item.CustomDate2 = collection.CustomDate2_state ? model.CustomDate2?.ToUniversalTime() : null;
            item.CustomDate3 = collection.CustomDate3_state ? model.CustomDate3?.ToUniversalTime() : null;

            collection.Items.Add(item);

            await SaveUpdates();

            return true;
        }

        public async Task<bool> UpdateItem(ItemPostModel model)
        {
            Item? item = (await _itemRepository.GetValue((i) => i.Id.Equals(model.Id))).FirstOrDefault();

            if (item == null) { return false; }

            MyCollection? collection = (await _collectionRepository
                .GetValue(x => x.Id == model.CollectionId))
                .FirstOrDefault();

            if (collection == null) { return false; }

            string[]? tagsStrings = model?.Tags?.Split(' ');

            if (tagsStrings == null) { return false; }

            tagsStrings = tagsStrings.Where(x => x.Replace(" ", "") != "").ToArray();

            List<Tag> tags = (await _tagRepository.GetAndCreate(tagsStrings, item)).ToList();

            item.Tags = tags;

            item.Name = model.Name;

            item.CustomText1 = collection.CustomText1_state ? model.CustomText1 : null;
            item.CustomText2 = collection.CustomText2_state ? model.CustomText2 : null;
            item.CustomText3 = collection.CustomText3_state ? model.CustomText3 : null;

            item.CustomString1 = collection.CustomString1_state ? model.CustomString1 : null;
            item.CustomString2 = collection.CustomString2_state ? model.CustomString2 : null;
            item.CustomString3 = collection.CustomString3_state ? model.CustomString3 : null;

            item.CustomInt1 = collection.CustomInt1_state ? model.CustomInt1 : null;
            item.CustomInt2 = collection.CustomInt2_state ? model.CustomInt2 : null;
            item.CustomInt3 = collection.CustomInt3_state ? model.CustomInt3 : null;

            item.CustomBool1 = collection.CustomBool1_state ? model.CustomBool1 : null;
            item.CustomBool2 = collection.CustomBool2_state ? model.CustomBool2 : null;
            item.CustomBool3 = collection.CustomBool3_state ? model.CustomBool3 : null;

            item.CustomDate1 = collection.CustomDate1_state ? model.CustomDate1?.ToUniversalTime() : null;
            item.CustomDate2 = collection.CustomDate2_state ? model.CustomDate2?.ToUniversalTime() : null;
            item.CustomDate3 = collection.CustomDate3_state ? model.CustomDate3?.ToUniversalTime() : null;

            await SaveUpdates();

            return true;
        }

        public async Task<List<Item>> GetItemsList(ItemsDataFilter filter,
                                               string value,
                                               DataSort sort,
                                               int page,
                                               int pageSize,
                                               string categoryName)
        {
            List<Item> items = (await _itemRepository
                .GetItemsList(filter, value, sort, page, pageSize, categoryName))
                .ToList();

            return items;
        }

        public async Task<int> GetItemsCount(ItemsDataFilter filter,
                                               string value,
                                               DataSort sort,
                                               string categoryName)
        {
            int count = await _itemRepository
                .GetItemsCount(filter, value, sort, categoryName);

            return count;
        }

        public async Task<List<Comment>> GetComentsList(Item item, ComentariesDataFilter filter, DateTime? fromTime = null)
        {
            return await _commentRepository.GetComentsList(item, filter, fromTime);
        }

        public async Task<Item?> GetById(string id)
        {
            return (await _itemRepository.GetValue((x) => x.Id == id)).FirstOrDefault();
        }

        public async Task DeleteRange(string[] itemsId, string userId)
        {
            await _itemRepository.DeleteRangeById(itemsId, userId);
        }

        public async Task SaveUpdates()
        {
            await _itemRepository.SaveUpdates();
        }
    }
}
