using CourseProject_backend.Builders;
using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
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

        public CollectionService
        (
            CollectionRepository repository,
            IConfiguration configuration,
            CollectionDBContext dbContext
        )
        {
            _dbContext = dbContext;
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<bool> CreateCollection(CollectionCreateModel model)
        {
            CollectionBuilder collectionBuilder = new CollectionBuilder(_configuration);

            User? authorUser = (await _dbContext.Users
                .Where((p) => p.Id == model.AuthorId)
                .ToListAsync()).FirstOrDefault();

            Category? category = (await _dbContext.Categories
                .Where((p) => p.Id == model.CategoryId)
                .ToListAsync()).FirstOrDefault();

            if (authorUser == null || category == null)
            {
                return false;
            }

            collectionBuilder.SetParameters(name: model.Name,
                                            description: model.Description,
                                            user: authorUser,
                                            category: category
                                            );

            MyCollection myCollection = collectionBuilder.Build() as MyCollection;

            await _repository.Add(myCollection);

            return true;
        }

        public async Task<bool> UpdateCollection(CollectionUpdateModel updateModel)
        {
            var collection = (await _repository.GetValue((c) => c.Id.Equals(updateModel.Id))).FirstOrDefault();

            if(collection == null) { return false; }

            collection.Name = updateModel.Name;
            collection.Description = updateModel.Description;
            collection.IsDeleted = updateModel.IsDeleted;

            collection.CustomText1_state = updateModel.CustomText1_name != null;
            collection.CustomText1_name = updateModel.CustomText1_name;
            collection.CustomText2_state = updateModel.CustomText2_name != null;
            collection.CustomText2_name = updateModel.CustomText2_name;
            collection.CustomText3_state = updateModel.CustomText3_name != null;
            collection.CustomText3_name = updateModel.CustomText3_name;

            collection.CustomString1_state = updateModel.CustomString1_name != null;
            collection.CustomString1_name = updateModel.CustomString1_name;
            collection.CustomString2_state = updateModel.CustomString2_name != null;
            collection.CustomString2_name = updateModel.CustomString2_name;
            collection.CustomString3_state = updateModel.CustomString3_name != null;
            collection.CustomString3_name = updateModel.CustomString3_name;

            collection.CustomInt1_state = updateModel.CustomInt1_name != null;
            collection.CustomInt1_name = updateModel.CustomInt1_name;
            collection.CustomInt2_state = updateModel.CustomInt2_name != null;
            collection.CustomInt2_name = updateModel.CustomInt2_name;
            collection.CustomInt3_state = updateModel.CustomInt3_name != null;
            collection.CustomInt3_name = updateModel.CustomInt3_name;

            collection.CustomBool1_state = updateModel.CustomBool1_name != null;
            collection.CustomBool1_name = updateModel.CustomBool1_name;
            collection.CustomBool2_state = updateModel.CustomBool2_name != null;
            collection.CustomBool2_name = updateModel.CustomBool2_name;
            collection.CustomBool3_state = updateModel.CustomBool3_name != null;
            collection.CustomBool3_name = updateModel.CustomBool3_name;

            await SaveUpdates();

            return true;
        }

        public async Task SaveUpdates()
        {
            await _repository.SaveUpdates();
        }
    }
}
