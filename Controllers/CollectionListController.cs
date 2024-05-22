using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Extensions;

namespace CourseProject_backend.Controllers
{
    public class CollectionListController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CollectionService _collectionService;
        private readonly UserService _userService;
        private int _pageSize = 20;

        public CollectionListController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] CollectionService collectionService,
            [FromServices] UserService userService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _configuration = configuration;
            _collectionService = collectionService;
            _userService = userService;

            _collectionService.Initialize(dBContext);
            _userService.Initialize(dBContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang = AppLanguage.en,
                                               CollectionDataFilter filter = CollectionDataFilter.byDefault,
                                               string value = "",
                                               DataSort sort = DataSort.byDefault,
                                               int page = 1,
                                               string categoryName = "")
        {
            this.DefineCategories();
            this.SetCollectionSearch();
            this.DefineCollectionSorts();

            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            User? user = null;

            if (Request.Cookies.TryGetValue("userData", out string? token))
            {
                user = await _userService.GetUserFromToken(token);
            }

            int pagesCount = 1;
            var collections = (await _collectionService.GetCollectionList
                              (filter, value, sort, page, _pageSize, categoryName)).ToList();

            if(!collections.IsNullOrEmpty())
            {
                pagesCount = (int)Math.Ceiling(await _collectionService
                             .GetCollectionsCount(filter, value, sort, categoryName) * 1.0 / (_pageSize * 1.0));
            }

            CollectionListViewModel viewModel = new CollectionListViewModel()
            {
                LanguagePack = langDataPair,
                Collections = collections,
                PagesCount = pagesCount,
                CurrentPage = page,
                Filter = filter,
                Sort = sort,
                Value = value,
                User = user
            };

            return View(viewModel);
        }
    }
}
