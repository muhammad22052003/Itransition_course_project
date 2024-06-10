using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using CourseProject_backend.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using CourseProject_backend.Entities;

namespace CourseProject_backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly int _topItemsCount = 10;
        private readonly int _topCollectionsCount = 5;

        private readonly IConfiguration _configuration;
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly CollectionService _collectionService;
        private readonly TagService _tagService;
        private readonly LanguagePackService _languagePackService;
        public HomeController
        (
            [FromServices] LanguagePackService languagePackService,
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService,
            [FromServices] ItemService itemService,
            [FromServices] CollectionService collectionService,
            [FromServices] CollectionDBContext dBContext,
            [FromServices] TagService tagService
        )
        {
            _languagePackService = languagePackService;
            _configuration = configuration;
            _collectionService = collectionService;
            _userService = userService;
            _itemService = itemService;
            _tagService = tagService;

            _collectionService.Initialize(dBContext);
            _userService.Initialize(dBContext);
            _itemService.Initialize(dBContext);
            _tagService.Initialize(dBContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index(AppLanguage lang = AppLanguage.en)
        {
            this.DefineCategories();
            this.SetItemSearch();
            this.DefineItemsSorts();

            if (Request.Cookies.TryGetValue("userData", out string? token))
            {
                User? user = await _userService.GetUserFromToken(token);

                if (user != null && user.IsAdmin()) { ViewData.Add("usersMenu", "show"); }
            }

            KeyValuePair<string, IDictionary<string, string>> langDataPair = _languagePackService.GetLanguagePackPair(lang);

            List<Item> items = (await _itemService
                .GetItemsList(filter: ItemsDataFilter.byDefault,
                                      value: "",
                                      sort: DataSort.byDate,
                                      page: 1,
                                      pageSize: _topItemsCount,
                                      categoryName: "")).ToList();

            List<MyCollection> collections = (await _collectionService
                .GetCollectionList(filter: CollectionDataFilter.byDefault,
                                      value: "",
                                      sort: DataSort.bySize,
                                      page: 1,
                                      pageSize: _topCollectionsCount,
                                      categoryName: "")).ToList();

            List<Tag> tags = (await _tagService.GetTagsList(filter: TagsDataFilter.byPopular,
                                                     value: "",
                                                     page: 1,
                                                     pageSize: 50)).ToList();

            HomeViewModel viewModel = new HomeViewModel()
            {
                Collections = collections,
                Items = items,
                LanguagePack = langDataPair,
                Tags = tags,
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Message(string message, AppLanguage lang = AppLanguage.en)
        {
            MessageViewModel viewModel = new MessageViewModel()
            {
                LangPack = _languagePackService.GetLanguagePackPair(lang),
                Message = message,
            };

            return View(viewModel);
        }
    }
}
