using CourseProject_backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject_backend.DbContexts
{
    public class MySqlContext : DbContext
    {
        private readonly string _connectionString;

        public MySqlContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Commentaries { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<MyCollection> Collections { get; set; }
        public DbSet<PositiveReaction> PositiveReactions { get; set; }
        public DbSet<NegativeReaction> NegativeReactions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
