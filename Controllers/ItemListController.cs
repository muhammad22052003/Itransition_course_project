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
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang,
                                               ItemsDataFilter filter,
                                               string value,
                                               DataSort sort,
                                               int page)
        {
            var langPackSingleton = LanguagePackSingleton.GetInstance();
            var langPackCollection = langPackSingleton.GetLanguagePack(lang);
            if (langPackCollection.IsNullOrEmpty()) { return NotFound(); }

            var langDataPair = new KeyValuePair
                               <string, IDictionary<string, string>>(lang.ToString(), langPackCollection);

            int pagesCount = 1;
            var items = (await _itemService
                              .GetItems(filter, value)).ToList();

            if (!items.IsNullOrEmpty())
            {
                items = _itemService.SortData(items, sort);
                pagesCount = (int)Math.Ceiling((double)(items.Count / 20.0));
                items = items.GetForPage(page, 20);
            }

            ItemsViewModel viewModel = new ItemsViewModel()
            {
                LanguagePack = langDataPair,
                Items = items,
                PagesCount = pagesCount,
                CurrentPage = page,
                Filter = filter,
                Sort = sort,
                Value = value
            };

            return View(viewModel);
        }
    }
}
