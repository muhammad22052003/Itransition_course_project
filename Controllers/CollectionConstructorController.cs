using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject_backend.Controllers
{
    public class CollectionConstructorController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly CollectionDBContext _dbContext;

        public CollectionConstructorController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _configuration = configuration;
            _userService = userService;
            _dbContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(AppLanguage lang = AppLanguage.en)
        {
            this.DefineCategories();
            this.SetCollectionSearch();

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return RedirectToAction("Index", "home", new { lang = lang.ToString() });
            }

            if (!await _userService.AuthorizationFromToken(token))
            {
                return RedirectToAction("index", "home", new { lang = lang.ToString()});
            }

            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            return View(langDataPair);
        }
    }
}
