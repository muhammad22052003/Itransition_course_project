using CourseProject_backend.Delegates;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.CustomDbContext;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

namespace CourseProject_backend.CustomDbContext
{
    public delegate void OnConfiguring(DbContextOptionsBuilder optionsBuilder);

    public class CollectionDBContext : DbContext
    {
        private readonly string _connectionString;

        public readonly DBSystem CurrentDbSystem;

        public CollectionDBContext(string connectionString, DBSystem dBSystem)
        {
            CurrentDbSystem = dBSystem;
            _connectionString = connectionString;

            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (CurrentDbSystem)
            {
                case DBSystem.MYSQL:
                    optionsBuilder.UseMySQL(_connectionString);
                    break;
                case DBSystem.POSTGRES:
                    optionsBuilder.UseNpgsql(_connectionString);
                    break;
                default: throw new ArgumentException("Such a database is not supported");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public LikeDelegate GetLikeDelegate()
        {
            switch (CurrentDbSystem)
            {
                case DBSystem.MYSQL:
                    return EF.Functions.Like;
                case DBSystem.POSTGRES:
                    return EF.Functions.ILike;
                default:
                    throw new NotImplementedException("Undefined DbSytem");
            }
               
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Commentaries { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<MyCollection> Collections { get; set; }
        public DbSet<PositiveReaction> PositiveReactions { get; set; }
        public DbSet<NegativeReaction> NegativeReactions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ViewModel> Views { get; set; }
    }
}
