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

            //collection.Items.Add(item);

            await _itemRepository.Add(item);

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
