﻿using CourseProject_backend.Entities;
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
            item.CreatedTime = DateTime.Now;
            item.Collection = null;
            item.IsDeleted = false;
            item.PositiveReact = new List<PositiveReaction>();
            item.NegativeReact = new List<NegativeReaction>();
            item.Comments = new List<Comment>();
            item.Tags = new List<Tag>();

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
            ICollection<Comment> comments = null,
            bool isDeleted = false
        )
        {
            Item item = new Item();

            item.Name = name;
            item.ImageUrl = imageUrl;
            item.Collection = collection;
            item.Tags = tags;
            item.PositiveReact = positiveReacts != null ? positiveReacts : item.PositiveReact;
            item.NegativeReact = negativeReacts != null ? negativeReacts : item.NegativeReact;
            item.Comments = comments != null ? comments : item.Comments;
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
