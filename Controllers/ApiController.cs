using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CourseProject_backend.Controllers
{
    public class ApiController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CollectionService _collectionService;
        private readonly UserService _userService;
        private readonly ItemService _itemService;

        public ApiController
        (
            IConfiguration configuration,
            CollectionService collectionService,
            UserService userService,
            ItemService itemService
        )
        {
            _configuration = configuration;
            _userService = userService;
            _collectionService = collectionService;
            _itemService = itemService;
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

            if(user.Role.ToLower() != UserRoles.Admin.ToString().ToLower() 
            && user.Role.ToLower() != UserRoles.User.ToString().ToLower())
            {
                return BadRequest("The user does not have access for this action");
            }

            if(!await _collectionService.CreateCollection(model, user))
            {
                return BadRequest("An error occurred, the collection was not created");
            }

            return Ok();
        }
    }
}
