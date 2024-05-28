using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Xml;

namespace CourseProject_backend.Controllers
{
    public class CollectionConstructorController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly CollectionService _collectionService;
        private readonly LanguagePackService _languagePackService;

        public CollectionConstructorController
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
        public async Task<IActionResult> Add(AppLanguage lang = AppLanguage.en)
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

            KeyValuePair<string, IDictionary<string, string>> langDataPair = _languagePackService.GetLanguagePackPair(lang);

            return View(langDataPair);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CollectionCreateModel model,
                                             AppLanguage lang = AppLanguage.en)
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

            if (!await _collectionService.CreateCollection(model,user))
            {
                return BadRequest("An error occurred, the collection was not created");
            }

            return RedirectToAction("index", "cabinet", new { lang = lang, id = user.Id});
        }

        [HttpGet]
        public async Task<IActionResult> Edit(AppLanguage lang = AppLanguage.en, string? id = null)
        {
            this.DefineCategories();
            this.SetCollectionSearch();
            this.DefineCollectionSorts();

            if(id.IsNullOrEmpty()) { return BadRequest("id empty"); }

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return RedirectToAction("Index", "home", new { lang = lang.ToString() });
            }

            if (!await _userService.AuthorizationFromToken(token))
            {
                return RedirectToAction("index", "home", new { lang = lang.ToString() });
            }

            MyCollection? collection = await _collectionService.GetById(id);

            if(collection == null) { return BadRequest("Collection undefined by id"); }

            KeyValuePair<string, IDictionary<string, string>> langDataPair = _languagePackService.GetLanguagePackPair(lang);

            CollectionConstructorViewModel viewModel = new CollectionConstructorViewModel()
            {
                LanguagePack = langDataPair,
                Collection = collection
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CollectionCreateModel model,
                                                          AppLanguage lang = AppLanguage.en)
        {
            if(model.CollectionId == null) { return NotFound(); }

            string? token = HttpContext.Request.Cookies["userData"];

            if (token.IsNullOrEmpty())
            {
                return BadRequest("User token undfined");
            }

            User? user = await _userService.GetUserFromToken(token);

            if(user == null) { return BadRequest("User undefined or collectionId empty"); }

            if (!user.IsAdmin())
            {
                if(!user.IsUser() || user.Collections.FirstOrDefault(x => x.Id == model.CollectionId) == null)
                {
                    return BadRequest("The user does not have access for this action");
                }
            }


            if (await _collectionService.UpdateCollection(model))
            {
                return RedirectToAction("index", "collection", new { lang = lang, id = model.CollectionId });
            }

            return BadRequest("Collection don`t updated");
        }
    }
}
