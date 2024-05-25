using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public static void DefineCollectionSorts(this Controller controller)
        {
            List<string> sorts = new List<string>()
            {
                //DataSort.byDefault.ToString(),
                DataSort.byName.ToString(),
                DataSort.bySize.ToString(),
                DataSort.byDate.ToString(),
            };

            controller.ViewData.Add("sorts", sorts);
        }

        public static void DefineUsersSorts(this Controller controller)
        {
            List<string> sorts = new List<string>()
            {
                //DataSort.byDefault.ToString(),
                DataSort.byName.ToString(),
                DataSort.byDate.ToString(),
            };

            controller.ViewData.Add("sorts", sorts);
        }

        public static void DefineItemsSorts(this Controller controller)
        {
            List<string> sorts = new List<string>()
            {
                //DataSort.byDefault.ToString(),
                DataSort.byName.ToString(),
                DataSort.byLike.ToString(),
                DataSort.byView.ToString(),
                DataSort.byDate.ToString(),
            };

            controller.ViewData.Add("sorts", sorts);
        }

        public static void SetCollectionSearch(this Controller controller)
        {
            controller.ViewData.Add("searchUrl", "search/collection");
        }

        public static void SetItemSearch(this Controller controller)
        {
            controller.ViewData.Add("searchUrl", "search/item");
        }

        public static void SetUserSearch(this Controller controller)
        {
            controller.ViewData.Add("searchUrl", "search/User");
        }

        public static KeyValuePair<string, IDictionary<string, string>> GetLanguagePackage(this Controller controller, 
                                                                                            AppLanguage lang)
        {
            var langPackSingleton = LanguagePackSingleton.GetInstance();
            var langPackCollection = langPackSingleton.GetLanguagePack(lang);

            KeyValuePair<string, IDictionary<string, string>> langDataPair = new KeyValuePair
                               <string, IDictionary<string, string>>(lang.ToString(), langPackCollection);

            return langDataPair;
        }
    }
}
