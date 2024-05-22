using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Enums;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.CustomDbContext;
using System.IO;

namespace CourseProject_backend.Controllers
{
    public class CollectionController : Controller
    {
        private readonly int _pageSize = 20;
        private readonly IConfiguration _configuration;
        private readonly ItemService _itemService;
        private readonly CollectionService _collectionService;
        private readonly UserService _userService;
        private readonly int pageSize = 20;

        public CollectionController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] ItemService itemService,
            [FromServices] CollectionService collectionService,
            [FromServices] UserService userService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _configuration = configuration;
            _itemService = itemService;
            _collectionService = collectionService;
            _userService = userService;

            _itemService.Initialize(dBContext);
            _collectionService.Initialize(dBContext);
            _userService.Initialize(dBContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang = AppLanguage.en,
                                               DataSort sort = DataSort.byDefault,
                                               string? id = null,
                                               int page = 1,
                                               string categoryName = "")
        {
            if(id == null) { return NotFound(); }

            this.DefineCategories();
            this.SetItemSearch();
            this.DefineCollectionSorts();

            User? user = null;

            if (Request.Cookies.TryGetValue("userData", out string? token))
            {
                user = await _userService.GetUserFromToken(token);
            }

            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            int pagesCount = 1;

            MyCollection? collection = await _collectionService.GetById(id);

            if(collection == null) { return NotFound(); }

            var items = (await _itemService.GetItemsList
                              (
                                filter: ItemsDataFilter.byCollectionId,
                                value: collection.Id,
                                sort: sort,
                                page: page,
                                pageSize: _pageSize,
                                categoryName: categoryName)).ToList();

            if (!items.IsNullOrEmpty())
            {
                pagesCount = (int)Math.Ceiling(await _itemService
                             .GetItemsCount
                             (
                                filter: ItemsDataFilter.byCollectionId,
                                value: collection.Id,
                                sort: sort,
                                categoryName: categoryName) * 1.0 / (_pageSize * 1.0));
            }

            CollectionViewModel viewModel = new CollectionViewModel()
            {
                LanguagePack = langDataPair,
                Items = items,
                PagesCount = pagesCount,
                CurrentPage = page,
                Sort = sort,
                Collection = collection,
                User = user
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] collectionId)
        {
            string? token = null;

            if(Request.Cookies.TryGetValue("userData", out token))
            {
                User? user = await _userService.GetUserFromToken(token);

                if(user == null)
                {
                    return NotFound();
                }

                await _collectionService.DeleteRange(collectionId, user.Id);

                return RedirectToAction("index", "cabinet");
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> ExportToCsv(string id)
        {
            MyCollection? collection = await _collectionService.GetById(id);

            if (collection == null) { return NotFound(); }

            byte[] bytes = _collectionService.GetCollectionCsv(collection);

            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "collection_table.csv");
        }
    }
}
