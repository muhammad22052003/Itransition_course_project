using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class MyProfileController : Controller
    {
        private readonly UserService _userService;

        public MyProfileController
        (
            [FromServices] UserService userService,
            [FromServices] CollectionDBContext dbContext
        )
        {
            _userService = userService;

            _userService.Initialize(dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index(AppLanguage lang = AppLanguage.en)
        {
            this.DefineCategories();
            this.SetItemSearch();
            this.DefineItemsSorts();

            User? user = null;

            if (Request.Cookies.TryGetValue("userData", out string? token))
            {
                user = await _userService.GetUserFromToken(token);
            }

            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            MyProfileViewModel viewModel = new MyProfileViewModel()
            {
                User = user,
                LanguagePack = langDataPair
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileModel model)
        {
            Request.Cookies.TryGetValue("userData", out string? token);

            if (token == null ||
                !ModelState.IsValid || 
                !await _userService.EditUserData(model, token))
            {
                return BadRequest(error : "Data invalid");
            }

            return Ok();
        }
    }
}
