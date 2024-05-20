using CourseProject_backend.CustomDbContext;
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
    public class ApiController : Controller
    {
        /*private readonly IConfiguration _configuration;
        private readonly CollectionService _collectionService;
        private readonly UserService _userService;
        private readonly ItemService _itemService;
        private readonly ReactionService _reactionService;

        public ApiController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] CollectionService collectionService,
            [FromServices] UserService userService,
            [FromServices] ItemService itemService,
            [FromServices] ReactionService reactionService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _configuration = configuration;
            _userService = userService;
            _collectionService = collectionService;
            _itemService = itemService;
            _reactionService = reactionService;

            _userService.Initialize(dBContext);
            _collectionService.Initialize(dBContext);
            _itemService.Initialize(dBContext);
            _reactionService.Initialize(dBContext);
        }*/
    }
}
