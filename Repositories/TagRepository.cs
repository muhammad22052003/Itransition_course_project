﻿using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseProject_backend.Repositories
{
    public class TagRepository : IRepository<Tag>
    {
        private readonly CollectionDBContext _dBContext;

        public TagRepository(CollectionDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<IEnumerable<Tag>> GetAndCreate(string[] tags, Item item)
        {
            List<Tag> resultTags = new List<Tag>();

            tags = tags.Distinct().ToArray();

            foreach (var tag in tags)

            {
                Tag newTag = new Tag(tag, item);

                resultTags.Add(newTag);
            }

            return resultTags;
        }

        public async Task Add(Tag tag)
        {
            _dBContext.Add(tag);

            await _dBContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetValue(Expression<Func<Tag, bool>> predicate)
        {
            return await _dBContext.Tags
                .Where(predicate)
                .ToArrayAsync();
        }

        public async Task Delete(Tag tag)
        {
            _dBContext.Remove(tag);

            await _dBContext.SaveChangesAsync();
        }

        public async Task SaveUpdates()
        {
            await _dBContext.SaveChangesAsync();
        }

        public IQueryable<Tag>? SortData(DataSort sort)
        {
            throw new NotImplementedException();
        }
    }
}
