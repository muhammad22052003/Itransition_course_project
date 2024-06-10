using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace CourseProject_backend.Controllers
{
    public class ItemController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly CollectionService _collectionService;
        private readonly ReactionService _reactionService;
        private readonly LanguagePackService _languagePackService;

        public ItemController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] ItemService itemService,
            [FromServices] UserService userService,
            [FromServices] CollectionService collectionService,
            [FromServices] CollectionDBContext dBContext,
            [FromServices] ReactionService reactionService,
            [FromServices] LanguagePackService languagePackService
        )
        {
            _languagePackService = languagePackService;
            _configuration = configuration;
            _itemService = itemService;
            _userService = userService;
            _collectionService = collectionService;
            _reactionService = reactionService;

            _itemService.Initialize(dBContext);
            _userService.Initialize(dBContext);
            _collectionService.Initialize(dBContext);
            _reactionService.Initialize(dBContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index(AppLanguage lang = AppLanguage.en, string? id = null)
        {
            this.DefineCategories();
            this.SetItemSearch();
            this.DefineItemsSorts();

            if (id == null) { return NotFound(); }

            KeyValuePair<string, IDictionary<string, string>> langDataPair = _languagePackService.GetLanguagePackPair(lang);

            Item? item = await _itemService.GetById(id);

            User? user = null;

            if (Request.Cookies.TryGetValue("userData", out string? token))
            {
                user = await _userService.GetUserFromToken(token);
            }

            MyCollection? collection = await _collectionService.GetByItemId(item.Id);

            if(collection == null) { throw new Exception("Error item without collection"); }

            ItemViewModel viewModel = new ItemViewModel()
            {
                Item = item,
                LanguagePack = langDataPair,
                User = user
            };

            ViewModel view = new ViewModel(user, item);

            item.Views.Add(view);
            await _itemService.SaveUpdates();

            ViewData.Add("currentCollection", collection.Name);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] itemId, AppLanguage lang)
        {
            string? token = null;

            if (Request.Cookies.TryGetValue("userData", out token))
            {
                User? user = await _userService.GetUserFromToken(token);

                if (user == null)
                {
                    return NotFound();
                }

                await _itemService.DeleteRange(itemId, user.Id);

                return RedirectToAction("index", "cabinet", new { lang = lang.ToString() });
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteLike(string? itemId = null)
        {
            if (itemId == null) { return BadRequest("Id not available or content is empty"); }

            Item? item = await _itemService.GetById(itemId);

            if (item == null) { return NotFound(); }

            string? token = HttpContext.Request.Cookies["userData"];

            if (token.IsNullOrEmpty())
            {
                return BadRequest("User token undfined");
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null)
            {
                return BadRequest("User undefined from token");
            }

            if (user.Role.ToLower() != UserRoles.Admin.ToString().ToLower()
            && user.Role.ToLower() != UserRoles.User.ToString().ToLower())
            {
                return BadRequest("The user does not have access for this action");
            }

            await _reactionService.DeleteByUserAndItem(user, item);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> AddLike(string? itemId = null)
        {
            if (itemId == null) { return BadRequest("Id not available or content is empty"); }

            Item? item = await _itemService.GetById(itemId);

            if (item == null) { return NotFound(); }

            string? token = HttpContext.Request.Cookies["userData"];

            if (token.IsNullOrEmpty())
            {
                return BadRequest("User token undfined");
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null)
            {
                return BadRequest("User undefined from token");
            }

            if (user.Role.ToLower() != UserRoles.Admin.ToString().ToLower()
            && user.Role.ToLower() != UserRoles.User.ToString().ToLower())
            {
                return BadRequest("The user does not have access for this action");
            }

            PositiveReaction like = new PositiveReaction("like", user, item);

            await _reactionService.AddReaction(like);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetItemComments([FromBody] GetCommentsPostModel model)
        {

            Item? item = await _itemService.GetById(model.ItemId);

            if (item == null) { return NotFound(); }

            var comentaries = await _itemService
                              .GetComentsList(item, ComentariesDataFilter.byFromTime, model.FromTime);

            if (comentaries.IsNullOrEmpty())
            {
                return NoContent();
            }

            List<CommentData> commentData = new List<CommentData>();

            for (int i = 0; i < comentaries.Count; i++)
            {
                commentData.Add(new CommentData()
                {
                    User = comentaries[i].User.Name,
                    Text = comentaries[i].Text,
                    Date = comentaries[i].CreatedTime.Date.ToShortDateString(),
                });
            }

            string json = JsonSerializer.Serialize(commentData);

            JsonDocument jsonDocument = JsonDocument.Parse(json);

            return Json(jsonDocument);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] AddCommentPostModel model)
        {
            if (model.ItemId == null || model.CommentText.IsNullOrEmpty()) { return BadRequest("Id not available or content is empty"); }

            Item? item = await _itemService.GetById(model.ItemId);

            if (item == null) { return NotFound(); }

            string? token = HttpContext.Request.Cookies["userData"];

            if (token.IsNullOrEmpty())
            {
                return BadRequest("User token undfined");
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null)
            {
                return BadRequest("User undefined from token");
            }

            if (user.Role.ToLower() != UserRoles.Admin.ToString().ToLower()
            && user.Role.ToLower() != UserRoles.User.ToString().ToLower())
            {
                return BadRequest("The user does not have access for this action");
            }

            Comment comment = new Comment(model.CommentText, user, item);

            item.Comments.Add(comment);

            await _itemService.SaveUpdates();

            return Ok();
        }
    }

    public struct AddCommentPostModel
    {
        public string ItemId { get; set; }
        public string CommentText { get; set; }
    }

    public struct GetCommentsPostModel
    {
        public string ItemId { get; set; }
        public DateTime FromTime { get; set; }
    }
}
