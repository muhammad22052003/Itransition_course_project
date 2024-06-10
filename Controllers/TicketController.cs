using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.CustomDbContext;

namespace CourseProject_backend.Controllers
{
    public class TicketController : Controller
    {
        private readonly JiraTicketService _ticketService;
        private readonly UserService _userService;
        private readonly LanguagePackService _languagePackService;

        public TicketController
        (
            JiraTicketService ticketService,
            UserService userService,
            LanguagePackService languagePackService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _userService = userService;
            _ticketService = ticketService;
            _languagePackService = languagePackService;

            _userService.Initialize(dBContext);
        }

        [HttpPost]
        public async Task<IActionResult> Index(JiraTicketCreateModel model, AppLanguage lang = AppLanguage.en)
        {
            this.DefineCategories();
            this.DefineItemsSorts();
            this.SetItemSearch();

            var langPack = _languagePackService.GetLanguagePackPair(lang);

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return RedirectToAction("message", "home", new { lang = lang, message = langPack.Value["you_must_authorized"] });
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null)
                return RedirectToAction("message", "home", new { lang = lang, message = langPack.Value["you_must_authorized"] });

            JiraTicketViewModel viewModel = new JiraTicketViewModel()
            {
                Collection = model.Collection,
                Link = model.Link,
                LangPack = langPack,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(JiraTicketCreateModel model, AppLanguage lang = AppLanguage.en)
        {
            var langPack = _languagePackService.GetLanguagePackPair(lang);

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return RedirectToAction("message", "home", new { lang = lang, message = langPack.Value["you_must_authorized"] });
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null)
            {
                return RedirectToAction("message", "home", new { lang = lang, message = langPack.Value["you_must_authorized"] });
            }

            if(!await _ticketService.AddTicketAsync(model, user))
            {
                return RedirectToAction("message", "home", new { lang = lang, message = langPack.Value["ticket_dont_created"] });
            }

            return RedirectToAction("index", "home", new { lang = lang });
        }
    }
}
