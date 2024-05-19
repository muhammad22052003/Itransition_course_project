using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Repositories;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace CourseProject_backend.Controllers
{
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

    public class ApiController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CollectionService _collectionService;
        private readonly UserService _userService;
        private readonly ItemService _itemService;
        private readonly ReactionService _reactionService;

        public ApiController
        (
            IConfiguration configuration,
            CollectionService collectionService,
            UserService userService,
            ItemService itemService,
            ReactionService reactionService
        )
        {
            _configuration = configuration;
            _userService = userService;
            _collectionService = collectionService;
            _itemService = itemService;
            _reactionService = reactionService;
        }

        [HttpPost]
        [Route("api/CreateCollection")]
        public async Task<IActionResult> CreateCollection([FromBody]CollectionCreateModel model)
        {
            string? token = HttpContext.Request.Cookies["userData"];

            if (token.IsNullOrEmpty())
            {
                return BadRequest("User token undfined");
            }

            User? user = await _userService.GetUserFromToken(token);

            if(user == null)
            {
                return BadRequest("User undefined from token");
            }

            if(!user.IsUser() && !user.IsAdmin())
            {
                return BadRequest("The user does not have access for this action");
            }

            if(!await _collectionService.CreateCollection(model, user))
            {
                return BadRequest("An error occurred, the collection was not created");
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetItemComments([FromBody]GetCommentsPostModel model)
        {

            Item? item = await _itemService.GetById(model.ItemId);

            if(item == null) { return NotFound(); }

            var comentaries = await _itemService
                              .GetComentsList(item, ComentariesDataFilter.byFromTime, model.FromTime);

            if(comentaries.IsNullOrEmpty()){
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
        public async Task<IActionResult> AddComment([FromBody]AddCommentPostModel model)
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

        
    }
}
