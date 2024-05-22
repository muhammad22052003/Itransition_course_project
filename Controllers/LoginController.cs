using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Packages;
using Microsoft.AspNetCore.Mvc;
using CourseProject_backend.Services;
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Extensions;
using CourseProject_backend.CustomDbContext;

namespace CourseProject_backend.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public LoginController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _configuration = configuration;
            _userService = userService;

            _userService.Initialize(dBContext);
        }

        [HttpGet]
        public IActionResult Index([FromRoute] AppLanguage lang = AppLanguage.en)
        {
            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            RegistrationModel registrationModel = new RegistrationModel()
            {
                Errors = new Dictionary<string, string>(),
                LanguagePack = langDataPair
            };

            return View(registrationModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang, UserLoginModel model)
        {
            var langPackSingleton = LanguagePackSingleton.GetInstance();
            var langPackCollection = langPackSingleton.GetLanguagePack(lang);
            if (langPackCollection.IsNullOrEmpty()) { return NotFound(); }

            var langDataPair = new KeyValuePair
                               <string, IDictionary<string, string>>(lang.ToString(), langPackCollection);

            var errorsDictionary = ModelState.GetErrors();
            RegistrationModel registrationModel = new RegistrationModel()
            {
                Errors = errorsDictionary,
                LanguagePack = langDataPair
            };
            if (!ModelState.IsValid)
            {
                return View(registrationModel);
            }

            string token = await _userService.Login(model);

            if (!token.IsNullOrEmpty())
            {
                Response.Cookies.Append("userData", token);
                return RedirectToAction("Index", "home", new { lang = lang });
            }
            else
            {
                errorsDictionary = ModelState.GetErrors();

                errorsDictionary.Add("Email", $"{langPackCollection["there_email_incorrect"]}");

                registrationModel.Errors = errorsDictionary;

                return View(registrationModel);
            }
        }

        public IActionResult Logout()
        {
            if (Request.Cookies.TryGetValue("userData", out string? token))
            {
                Response.Cookies.Delete("userData");
            }
            return RedirectToAction("index", "start");
        }
    }
}
