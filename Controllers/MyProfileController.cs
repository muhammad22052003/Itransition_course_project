using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using CustomJiraTicketClient.Jira;
using CustomJiraTicketClient.Jira.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class MyProfileController : Controller
    {
        private readonly UserService _userService;
        private readonly LanguagePackService _languagePackService;
        private readonly JiraTicketService _ticketService;
        private readonly int _ticketsPageSize = 12;
        private readonly IConfiguration _configuration;

        public MyProfileController
        (
            [FromServices] UserService userService,
            [FromServices] CollectionDBContext dbContext,
            [FromServices] LanguagePackService languagePackService,
            [FromServices] JiraTicketService ticketService,
            [FromServices] IConfiguration configuration
        )
        {
            _ticketService = ticketService;
            _userService = userService;
            _languagePackService = languagePackService;
            _configuration = configuration;

            _userService.Initialize(dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index(AppLanguage lang = AppLanguage.en)
        {
            this.DefineCategories();
            this.SetItemSearch();
            this.DefineItemsSorts();

            User? user = null;

            KeyValuePair<string, IDictionary<string, string>> langDataPair = _languagePackService.GetLanguagePackPair(lang);

            if (Request.Cookies.TryGetValue("userData", out string? token))
            {
                user = await _userService.GetUserFromToken(token);
            }
            else
                return RedirectToAction("message", "home", new { lang = lang, message = langDataPair.Value["you_must_authorized"] });

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

        [HttpGet]
        public async Task<IActionResult> MyTickets(AppLanguage lang = AppLanguage.en, int page = 1)
        {
            page = page == 0 ? 1 : page;

            var langPack = _languagePackService.GetLanguagePackPair(lang);

            if (!Request.Cookies.TryGetValue("userData", out string? token))
                return RedirectToAction("message", "home", new { lang = lang, message = langPack.Value["you_must_authorized"] });

            List<JiraTicket> tickets = new List<JiraTicket>();

            var user = await _userService.GetUserFromToken(token);

            if (user == null) { return NotFound(); }

            int currentPage = page;

            tickets = (await _ticketService.GetUserTickets(email: user.Email,
                                                               startAt: (page - 1) * _ticketsPageSize,
                                                               maxCount: _ticketsPageSize)).ToList();

            MyTicketsViewModel viewModel = new MyTicketsViewModel()
            {
                Tickets = tickets,
                LangPack = langPack,
                User = user,
                CurrentPage = currentPage,
                PageSize = _ticketsPageSize,
                BaseTicketsUrl = _configuration.GetValue<string>("jiraTicketBaseUrl")
            };

            return View(viewModel);
        }
    }
}
