using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseProject_backend.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly CollectionDBContext _dBContext;

        public CommentRepository(CollectionDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task Add(Comment comment)
        {
            _dBContext.Add(comment);

            await _dBContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetValue(Expression<Func<Comment, bool>> predicat)
        {
            return await _dBContext.Commentaries
                .Where(predicat)
                .ToArrayAsync();
        }

        public async Task<List<Comment>> GetComentsList(Item item, ComentariesDataFilter filter, DateTime? fromTime = null)
        {
            List<Comment> comments = new List<Comment>();

            switch (filter)
            {
                case ComentariesDataFilter.byDefault:
                    {
                        comments = await _dBContext.Commentaries
                            .Where(it => it.Item.Id == item.Id)
                            .OrderBy(c => c.CreatedTime)
                            .ToListAsync();
                    }
                    break;
                case ComentariesDataFilter.byFromTime:
                    {
                        if (fromTime == null) { throw new ArgumentNullException(nameof(fromTime)); }

                        comments = await _dBContext.Commentaries
                           .Where(c => c.Item.Id == item.Id && c.CreatedTime >= fromTime.Value)
                           .OrderBy(c => c.CreatedTime)
                           .ToListAsync();
                    }
                    break;
            }

            return comments;
        }

        public async Task Delete(Comment comment)
        {
            _dBContext.Remove(comment);

            await _dBContext.SaveChangesAsync();
        }

        public async Task SaveUpdates()
        {
            await _dBContext.SaveChangesAsync();
        }

        public IQueryable<Comment>? SortData(DataSort sort)
        {
            throw new NotImplementedException();
        }
    }
}
