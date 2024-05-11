using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace CourseProject_backend.Controllers
{
    public class ApiUserController : Controller
    {
        private readonly UserService _userService;
        private readonly CollectionDBContext _dBContext;
        private readonly IConfiguration _configuration;

        public ApiUserController
        (
            [FromServices]UserService userService,
            [FromServices]CollectionDBContext dBContext,
            IConfiguration configuration
        )
        {
            _userService = userService;
            _dBContext = dBContext;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserLoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = await _userService.Login(model);

            if(token.IsNullOrEmpty())
            {
                return Conflict("User No Loggined");
            }

            Response.Cookies.Append("userDataBytech",token);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromBody]UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool userAdded = await _userService.Registration(model);

            if(userAdded)
            {
                return Ok();
            }

            return Conflict("Conflict User No added");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
