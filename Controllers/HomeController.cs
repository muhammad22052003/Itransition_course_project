using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models;
using CourseProject_backend.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace CourseProject_backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController
        (
            [FromServices] IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(AppLanguage lang = AppLanguage.en)
        {
            this.DefineCategories();
            this.SetItemSearch();

            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            return View(langDataPair);
        }
    }
}
