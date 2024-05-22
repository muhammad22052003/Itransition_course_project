using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject_backend.Controllers
{
    public class ItemConstructorController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly ItemService _itemService;
        private readonly CollectionService _collectionService;


        public ItemConstructorController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService,
            [FromServices] CollectionDBContext dBContext,
            [FromServices] ItemService itemService,
            [FromServices] CollectionService collectionService
        )
        {
            _configuration = configuration;
            _userService = userService;
            _itemService = itemService;
            _collectionService = collectionService;

            _userService.Initialize(dBContext);
            _itemService.Initialize(dBContext);
            _collectionService.Initialize(dBContext);
        }

        [HttpGet]
        public async Task<IActionResult> Add([FromRoute]AppLanguage lang = AppLanguage.en,
                                               string? id = null)
        {

            if (id == null) { return BadRequest("The request has no ID"); }

            this.DefineCategories();
            this.SetItemSearch();
            this.DefineItemsSorts();

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return RedirectToAction("Index", "home", new { lang = lang.ToString() });
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null)
            {
                return RedirectToAction("index", "home", new { lang = lang.ToString() });
            }

            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            MyCollection? collection = await _collectionService.GetById(id);

            if (collection == null) { return NotFound(); }

            if (!user.IsAdmin() && 
                user.Collections.FirstOrDefault((x)=>x.Id == id) == null)
            {
                return BadRequest("You do not have access for this operation");
            }

            ItemConstructorViewModel constructorViewModel = new ItemConstructorViewModel()
            {
                LanguagePack = langDataPair,
                Collection = collection
            };

            return View(constructorViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromRoute] AppLanguage lang ,ItemPostModel model)
        {
            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return NotFound();
            }

            User? user = await _userService.GetUserFromToken(token);

            MyCollection? collection = await _collectionService.GetById(model.CollectionId);

            if(collection == null) { return BadRequest("Collection not founded"); }

            if (user.Role.ToLower() != UserRoles.Admin.ToString().ToLower() &&
                user.Collections.FirstOrDefault((x) => x.Id == collection.Id) == null)
            {
                return BadRequest("You do not have access for this operation");
            }

            if(!await _itemService.CreateItem(model))
            {
                return Problem("An error occurred. Item was not created.");
            }

            return RedirectToAction("index", "Home", lang);
        }


        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] AppLanguage lang = AppLanguage.en,
                                               string? id = null)
        {
            if (id == null) { return BadRequest("The request has no ID"); }

            this.DefineCategories();
            this.SetItemSearch();

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return RedirectToAction("Index", "home", new { lang = lang.ToString() });
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null)
            {
                return RedirectToAction("index", "home", new { lang = lang.ToString() });
            }

            var langPackSingleton = LanguagePackSingleton.GetInstance();
            var langPackCollection = langPackSingleton.GetLanguagePack(lang);
            if (langPackCollection.IsNullOrEmpty()) { return NotFound(); }

            var langDataPair = new KeyValuePair
                               <string, IDictionary<string, string>>(lang.ToString(), langPackCollection);

            Item? item = await _itemService.GetById(id);

            if (item == null) { return NotFound(); }

            if (user.Role.ToLower() != UserRoles.Admin.ToString().ToLower() &&
                user.Collections
                .Where((c)=>c.Items.FirstOrDefault((it)=>it.Id == id) != null)
                .FirstOrDefault() == null)
            {
                return BadRequest("You do not have access for this operation");
            }

            ItemEditViewModel editViewModel = new ItemEditViewModel()
            {
                LanguagePack = langDataPair,
                Item = item
            };

            return View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] AppLanguage lang, [FromForm]ItemPostModel model)
        {
            if (!Request.Cookies.TryGetValue("userData", out string? token) ||
                model.Id == null)
            {
                return NotFound();
            }

            User? user = await _userService.GetUserFromToken(token);

            if(user == null)
            {
                return RedirectToAction("index", "home", new { lang = lang.ToString() });
            }

            if (user.Role.ToLower() != UserRoles.Admin.ToString().ToLower() &&
                user.Collections
                .Where((c) => c.Items.FirstOrDefault((it) => it.Id == model.Id) != null)
                .FirstOrDefault() == null)
            {
                return BadRequest("You do not have access for this operation");
            }

            if (!await _itemService.UpdateItem(model))
            {
                return Conflict("Error! Item don`t updated");
            }

            return RedirectToAction("index", "item", new
            {
                lang = lang,
                id = model.Id
            });
        }
    }
}
