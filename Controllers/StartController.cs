using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject_backend.Controllers
{
    public class StartController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly LanguagePackService _languagePackService;

        public StartController
        (
            IConfiguration configuration,
            [FromServices] LanguagePackService languagePackService
        )
        {
            _languagePackService = languagePackService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index(AppLanguage lang = AppLanguage.en)
        {
            this.DefineCategories();

            KeyValuePair<string, IDictionary<string,string>> langDataPair = _languagePackService.GetLanguagePackPair(lang);

            return View(langDataPair);
        }
    }
}
