using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject_backend.Controllers
{
    public struct RegistrationModel
    {
        public KeyValuePair<string, IDictionary<string, string>> LanguagePack { get; set;}

        public IDictionary<string, string> Errors { get; set; }
    }

    public class RegistrationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public RegistrationController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService
        )
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index([FromRoute]AppLanguage lang = AppLanguage.en)
        {
            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            RegistrationModel registrationModel = new RegistrationModel()
            {
                Errors = new Dictionary<string,string>(),
                LanguagePack = langDataPair
            };

            return View(registrationModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang, UserRegistrationModel model)
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
            if(await _userService.Registration(model))
            {
                UserLoginModel loginModel = new UserLoginModel()
                {
                    Email = model.Email,
                    Password = model.Password
                };
                
                string token = await _userService.Login(loginModel);
                Response.Cookies.Append("userData", token);
                return RedirectToAction("Index", "home", new { lang = lang });
            }
            else
            {
                errorsDictionary = ModelState.GetErrors();

                errorsDictionary.Add("Email", $"{langPackCollection["there_email"]}");

                registrationModel.Errors = errorsDictionary;

                return View(registrationModel);
            }
        }
    }
}
