using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Enums;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using CourseProject_backend.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject_backend.Controllers
{
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public UsersController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService
        )
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang,
                                               ItemsDataFilter filter,
                                               string value,
                                               DataSort sort,
                                               int page)
        {
            var langPackSingleton = LanguagePackSingleton.GetInstance();
            var langPackCollection = langPackSingleton.GetLanguagePack(lang);
            if (langPackCollection.IsNullOrEmpty()) { return NotFound(); }

            var langDataPair = new KeyValuePair
                               <string, IDictionary<string, string>>(lang.ToString(), langPackCollection);

            int pagesCount = 1;
            var users = (await _userService
                              .GetUsers(filter, value)).ToList();

            if (!users.IsNullOrEmpty())
            {
                users = _userService.SortData(users, sort);
                pagesCount = (int)Math.Ceiling((double)(users.Count / 20.0));
                users = users.GetForPage(page, 20);
            }

            UsersViewModel viewModel = new ItemsViewModel()
            {
                LanguagePack = langDataPair,
                Items = users,
                PagesCount = pagesCount,
                CurrentPage = page,
                Filter = filter,
                Sort = sort,
                Value = value
            };

            return View(viewModel);
        }
    }
}
