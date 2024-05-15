using CourseProject_backend.CustomDbContext;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace CourseProject_backend.Packages
{
    public static class CategoriesPackage
    {
        private static IEnumerable<string> CategoriesNames { get; set; } = new List<string>();

        public async static Task Initialize(CollectionDBContext dBContext)
        {
            CategoriesNames = (await dBContext.Categories
                .Where((c)=>true)
                .ToListAsync())
                .Select((c) => c.Name)
                .ToArray();
        }

        public static IEnumerable<string> GetCategoriesNames()
        {
            return CategoriesNames;
        }
    }
}
