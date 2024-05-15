using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Extensions;

namespace CourseProject_backend.Controllers
{
    public class ItemListController : Controller
    {
        private readonly int _pageSize = 20;
        private readonly IConfiguration _configuration;
        private readonly ItemService _itemService;

        public ItemListController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] ItemService itemService
        )
        {
            _configuration = configuration;
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang = AppLanguage.en,
                                               ItemsDataFilter filter = ItemsDataFilter.byTag,
                                               string value = "all",
                                               DataSort sort = DataSort.byDefault,
                                               int page= 1)
        {
            this.DefineCategories();
            this.SetItemSearch();

            var langPackSingleton = LanguagePackSingleton.GetInstance();
            var langPackCollection = langPackSingleton.GetLanguagePack(lang);
            if (langPackCollection.IsNullOrEmpty()) { return NotFound(); }

            var langDataPair = new KeyValuePair
                               <string, IDictionary<string, string>>(lang.ToString(), langPackCollection);

            int pagesCount = 1;
            var items = (await _itemService.GetItemsList
                              (filter, value, sort, page, _pageSize)).ToList();

            if (!items.IsNullOrEmpty())
            {
                pagesCount = (int)Math.Ceiling(await _itemService
                             .GetItemsCount(filter, value, sort) * 1.0 / (_pageSize * 1.0));
            }

            ItemsViewModel viewModel = new ItemsViewModel()
            {
                LanguagePack = langDataPair,
                Items = items,
                PagesCount = pagesCount,
                CurrentPage = page,
                Filter = filter,
                Sort = sort,
                Value = value,
            };

            return View(viewModel);
        }
    }
}
