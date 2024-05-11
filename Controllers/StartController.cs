using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject_backend.Controllers
{
    public class StartController : Controller
    {
        private readonly IConfiguration _configuration;

        public StartController
        (
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index(int lang = 0)
        {
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
