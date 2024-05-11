using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Enums.Packages;
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

        public async Task<IActionResult> Index(int lang = 0)
        {
            if (!Request.Cookies.TryGetValue("userDataBytech", out string? token))
            {
                return RedirectToAction("index", "start", lang);
            }

            if (!await _userService.AuthorizationFromToken(token))
            {
                return RedirectToAction("index", "start", lang);
            }

            var langPackSingleton = LanguagePackSingleton.GetInstance();

            try
            {
                var langPackCollection = langPackSingleton.GetLanguagePack((AppLanguage)lang);

                if (langPackCollection.IsNullOrEmpty()) { return NotFound(); }

                var langDataPair = new KeyValuePair
                                   <int, IDictionary<string, string>>(lang, langPackCollection);

                return View(langDataPair);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
