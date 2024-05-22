using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.RequestModels;
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
        private readonly CollectionService _collectionService;
        private readonly CollectionDBContext _dbContext;

        public CollectionConstructorController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService,
            [FromServices] CollectionService collectionService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _configuration = configuration;
            _userService = userService;
            _dbContext = dBContext;
            _collectionService = collectionService;

            _userService.Initialize(dBContext);
            _collectionService.Initialize(dBContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index(AppLanguage lang = AppLanguage.en)
        {
            this.DefineCategories();
            this.SetCollectionSearch();
            this.DefineCollectionSorts();

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

        [HttpPost]
        public async Task<IActionResult> CreateCollection([FromBody] CollectionCreateModel model)
        {
            string? token = HttpContext.Request.Cookies["userData"];

            if (token.IsNullOrEmpty())
            {
                return BadRequest("User token undfined");
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null)
            {
                return BadRequest("User undefined from token");
            }

            if (!user.IsUser() && !user.IsAdmin())
            {
                return BadRequest("The user does not have access for this action");
            }

            if (!await _collectionService.CreateCollection(model, user))
            {
                return BadRequest("An error occurred, the collection was not created");
            }

            return Ok();
        }
    }
}
