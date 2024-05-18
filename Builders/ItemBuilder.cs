using CourseProject_backend.Entities;
using CourseProject_backend.Interfaces;
using CourseProject_backend.Interfaces.Entities;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CourseProject_backend.Builders
{
    public class ItemBuilder : IBuilder
    {
        protected IDBModel _model { get; set; }
        protected IConfiguration _configuration { get; set; }

        public ItemBuilder(IConfiguration configuration = null)
        {
            _configuration = configuration; 

            Item item = new Item();

            item.Id = Guid.NewGuid().ToString().Replace("-","");
            item.Name = "untitled";
            try { item.ImageUrl = _configuration.GetValue<string>("urls:no_image"); }
            catch (Exception ex) { item.ImageUrl = ""; }
            item.CreatedTime = DateTime.UtcNow;
            item.Collection = null;
            item.IsDeleted = false;
            item.PositiveReact = new List<PositiveReaction>();
            item.Comments = new List<Comment>();
            item.Tags = new List<Tag>();
            item.Views = new List<ViewModel>();

            _model = item;
        }

        public void SetParameters
        (
            string name,
            MyCollection collection,
            string imageUrl = null,
            ICollection<Tag> tags = null,
            ICollection<PositiveReaction> positiveReacts = null,
            ICollection<NegativeReaction> negativeReacts = null,
            ICollection<ViewModel> views = null,
            ICollection<Comment> comments = null,
            bool isDeleted = false
        )
        {
            Item item = _model as Item;

            item.Name = name;
            item.ImageUrl = imageUrl;
            item.Collection = collection;
            item.Tags = tags != null ? tags : item.Tags;
            item.PositiveReact = positiveReacts != null ? positiveReacts : item.PositiveReact;
            item.Comments = comments != null ? comments : item.Comments;
            item.Views = views != null ? views : item.Views;
            item.IsDeleted = isDeleted;

            _model = item;
        }

        public IDBModel Build()
        {
            if(_model == null)
            {
                throw new Exception("Error trying retry build");
            }

            IDBModel result = _model;
            _model = null;
            return result;
        }
    }
}
