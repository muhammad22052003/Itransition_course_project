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
        private readonly int _pageSize = 20;

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
                                               UsersDataFilter filter,
                                               string value,
                                               DataSort sort,
                                               int page = 1)
        {
            var langPackSingleton = LanguagePackSingleton.GetInstance();
            var langPackCollection = langPackSingleton.GetLanguagePack(lang);
            if (langPackCollection.IsNullOrEmpty()) { return NotFound(); }

            var langDataPair = new KeyValuePair
                               <string, IDictionary<string, string>>(lang.ToString(), langPackCollection);

            int pagesCount = 1;
            var users = (await _userService.GetUsersList
                              (filter, value, sort, page, _pageSize)).ToList();

            if (!users.IsNullOrEmpty())
            {
                pagesCount = (int)Math.Ceiling(await _userService
                             .GetUsersCount(filter, value, sort) * 1.0 / (_pageSize * 1.0));
            }

            UsersViewModel viewModel = new UsersViewModel()
            {
                LanguagePack = langDataPair,
                Users = users,
                PagesCount = pagesCount,
                CurrentPage = page,
                Filter = filter,
                Sort = sort,
                Value = value,
            };

            return View(viewModel);
        }
    }
}
