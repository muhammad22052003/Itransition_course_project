using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Packages;
using Microsoft.AspNetCore.Mvc;
using CourseProject_backend.Services;
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Extensions;
using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Helpers;
using Google.Apis.Oauth2.v2.Data;
using Newtonsoft.Json.Linq;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2;

namespace CourseProject_backend.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly AppSecrets _appSecrets;
        private readonly LanguagePackService _languagePackService;
        private string _redirectUri; 

        public LoginController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService,
            [FromServices] CollectionDBContext dBContext,
            [FromServices] LanguagePackService languagePackService,
            [FromServices] AppSecrets appSecrets
        )
        {
            _languagePackService = languagePackService;
            _configuration = configuration;
            _userService = userService;
            _appSecrets = appSecrets;

            _userService.Initialize(dBContext);
        }

        [HttpGet]
        public IActionResult Index([FromRoute] AppLanguage lang = AppLanguage.en)
        {
            _redirectUri = "https" + "://" + Request.Host + "/login/GoogleAuth";

            KeyValuePair<string, IDictionary<string, string>> langDataPair = _languagePackService.GetLanguagePackPair(lang);

            string redirectUri = _redirectUri;

            string googleAuthUri = _userService.GetGoogleAuthUri(redirectUri);

            RegistrationModel registrationModel = new RegistrationModel()
            {
                Errors = new Dictionary<string,string>(),
                LanguagePack = langDataPair,
                GoogleAuthUri = googleAuthUri
            };

            return View(registrationModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang, UserLoginModel model)
        {
            _redirectUri = "https" + "://" + Request.Host + "/login/GoogleAuth";

            var langDataPair = _languagePackService.GetLanguagePackPair(lang);

            var errorsDictionary = ModelState.GetErrors();

            string redirectUri = _redirectUri;

            string googleAuthUri = _userService.GetGoogleAuthUri(redirectUri);

            RegistrationModel registrationModel = new RegistrationModel()
            {
                Errors = errorsDictionary,
                LanguagePack = langDataPair,
                GoogleAuthUri = googleAuthUri
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

                errorsDictionary.Add("Email", $"{langDataPair.Value["there_email_incorrect"]}");

                registrationModel.Errors = errorsDictionary;

                return View(registrationModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GoogleAuth(string code)
        {
            _redirectUri = "https" + "://" + Request.Host + "/login/GoogleAuth";

            string redirectUri = _redirectUri;

            string? token = await _userService.GoogleLogin(code, redirectUri);

            if (token.IsNullOrEmpty())
            {
                return BadRequest();
            }
            else
            {
                Response.Cookies.Append("userData", token);
                return RedirectToAction("Index", "home");
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
