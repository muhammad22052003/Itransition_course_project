using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject_backend.Controllers
{
    public class ItemController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly CollectionService _collectionService;

        public ItemController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] ItemService itemService,
            [FromServices] UserService userService,
            [FromServices] CollectionService collectionService
        )
        {
            _configuration = configuration;
            _itemService = itemService;
            _userService = userService;
            _collectionService = collectionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(AppLanguage lang = AppLanguage.en, string? id = null)
        {
            this.DefineCategories();
            this.SetItemSearch();

            if (id == null) { return NotFound(); }

            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            Item? item = await _itemService.GetById(id);

            User? user = null;

            if (Request.Cookies.TryGetValue("userData", out string? token))
            {
                user = await _userService.GetUserFromToken(token);
            }

            MyCollection? collection = await _collectionService.GetByItemId(item.Id);

            if(collection == null) { throw new Exception("Error item without collection"); }

            ItemViewModel viewModel = new ItemViewModel()
            {
                Item = item,
                LanguagePack = langDataPair,
                User = user
            };

            ViewModel view = new ViewModel(user, item);

            item.Views.Add(view);
            await _itemService.SaveUpdates();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] itemId)
        {
            string? token = null;

            if (Request.Cookies.TryGetValue("userData", out token))
            {
                User? user = await _userService.GetUserFromToken(token);

                if (user == null)
                {
                    return NotFound();
                }

                await _itemService.DeleteRange(itemId, user.Id);

                return RedirectToAction("index", "cabinet");
            }

            return NotFound();
        }
    }
}
