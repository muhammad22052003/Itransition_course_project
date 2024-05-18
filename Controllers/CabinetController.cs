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
        private readonly int _pageSize = 20;

        public CabinetController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService,
            [FromServices] CollectionService collectionService
        )
        {
            _configuration = configuration;
            _userService = userService;
            _collectionService = collectionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang = AppLanguage.en,
                                               string id = "",
                                               DataSort sort = DataSort.byDate,
                                               int page = 1,
                                               string categoryName = "")
        {
            this.DefineCategories();
            this.SetItemSearch();

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return RedirectToAction("index", "start", lang);
            }

            User? user = await _userService.GetUserFromToken(token);

            if(user == null) { return NotFound(); }

            KeyValuePair<string, IDictionary<string,string>> langDataPair = this.GetLanguagePackage(lang);

            int pagesCount = 1;

            List<MyCollection> collections = user.Collections
                .Skip((page - 1) * _pageSize)
                .OrderByDescending((x) => x.CreatedTime)
                .Take(_pageSize).ToList();

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

            return View(viewModel);
        }
    }
}
