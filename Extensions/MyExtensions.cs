using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace CourseProject_backend.Extensions
{
    public static class MyExtensions
    {
        public static List<TModel> GetForPage<TModel>(this List<TModel> list, int page, int count){
            List<TModel> newCollections = new List<TModel>();

            int startIndex = (page - 1) * 10;

            for (int i = startIndex; i < list.Count && i - startIndex <= count; i++)
            {
                newCollections.Add(list[i]);
            }

            return list;
        }

        public static IDictionary<string, string> GetErrors(this ModelStateDictionary modelState)
        {
            IDictionary<string, string> errorsDictionary = new Dictionary<string, string>();

            foreach (var item in modelState)
            {
                if (item.Value.Errors.Count != 0)
                {
                    errorsDictionary.Add(item.Key, item.Value.Errors[0].ErrorMessage);
                }
            }

            return errorsDictionary;
        }

        public static void DefineCategories(this Controller controller)
        {
            var Categories = CategoriesPackage.GetCategoriesNames();

            controller.ViewData.Add("categories", Categories.ToList());
        }

        public static void SetCollectionSearch(this Controller controller)
        {
            controller.ViewData.Add("searchUrl", "search/collection");
        }

        public static void SetItemSearch(this Controller controller)
        {
            controller.ViewData.Add("searchUrl", "search/item");
        }
    }
}
