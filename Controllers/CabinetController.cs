using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject_backend.Controllers
{
    public class CabinetController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly CollectionService _collectionService;
        private readonly LanguagePackService _languagePackService;
        private readonly int _pageSize = 20;

        public CabinetController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService,
            [FromServices] CollectionService collectionService,
            [FromServices] LanguagePackService languagePackService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _languagePackService = languagePackService;
            _configuration = configuration;
            _userService = userService;
            _collectionService = collectionService;

            _userService.Initialize(dBContext);
            _collectionService.Initialize(dBContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang = AppLanguage.en,
                                               string id = "",
                                               DataSort sort = DataSort.byDate,
                                               int page = 1,
                                               string categoryName = "")
        {
            this.DefineCategories();
            this.DefineItemsSorts();
            this.SetItemSearch();

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return RedirectToAction("index", "start", lang);
            }

            User? user = await _userService.GetUserFromToken(token);

            if(user == null) { return NotFound(); }

            KeyValuePair<string, IDictionary<string,string>> langDataPair = _languagePackService.GetLanguagePackPair(lang);

            int pagesCount = 1;

            List<MyCollection> collections = (await _collectionService
                .GetCollectionList(filter: CollectionDataFilter.byAuthorId,
                                   value: user.Id,
                                   DataSort.byDefault,
                                   page: page,
                                   pageSize: _pageSize,
                                   categoryName: categoryName)).ToList();

            if (!collections.IsNullOrEmpty())
            {
                pagesCount = (int)Math.Ceiling(user.Collections.ToList().Count * 1.0 / (_pageSize * 1.0));
            }

            CabinetViewModel viewModel = new CabinetViewModel()
            {
                LanguagePack = langDataPair,
                User = user,
                Collections = collections,
                CurrentPage = page,
                PagesCount = pagesCount
            };

            ViewData.Add("currentCategory", categoryName);
            ViewData.Add("currentSort", sort.ToString());

            return View(viewModel);
        }
    }
}
