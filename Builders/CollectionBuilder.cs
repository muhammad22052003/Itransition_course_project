using CourseProject_backend.Entities;
using CourseProject_backend.Interfaces;
using CourseProject_backend.Interfaces.Entities;

namespace CourseProject_backend.Builders
{
    public class CollectionBuilder : IBuilder
    {
        protected IDBModel _model { get; set; }
        protected IConfiguration _configuration { get; set; }

        public CollectionBuilder(IConfiguration configuration)
        {
            _configuration = configuration;

            MyCollection collection = new MyCollection();

            collection.Id = Guid.NewGuid().ToString().Replace("-", "");
            collection.Name = "untitled";
            try { collection.ImageUrl = _configuration.GetValue<string>("urls:no_image"); }
            catch (Exception ex) { collection.ImageUrl = ""; }
            collection.Description = "no description";
            collection.CreatedTime = DateTime.Now;
            collection.IsDeleted = false;

            _model = collection;
        }

        public void SetParameters
        (
            string name,
            string imageUrl,
            string description,
            User user,
            Category category,
            bool isDeleted = false
        )
        {
            MyCollection collection = new MyCollection();

            collection.Name = name;
            collection.ImageUrl = imageUrl;
            collection.Description = description;
            collection.User = user;
            collection.Category = category;
            collection.IsDeleted = isDeleted;

            _model = collection;
        }

        public IDBModel Build()
        {
            if (_model == null)
            {
                throw new Exception("Error trying retry build");
            }

            IDBModel result = _model;
            _model = null;
            return result;
        }
    }
}
