﻿using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Enums;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using CourseProject_backend.Extensions;
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Entities;

namespace CourseProject_backend.Controllers
{
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly int _pageSize = 20;

        public UsersController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] UserService userService
        )
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang,
                                               UsersDataFilter filter,
                                               string value,
                                               DataSort sort,
                                               int page = 1)
        {
            this.DefineCategories();
            this.SetItemSearch();

            KeyValuePair<string, IDictionary<string, string>> langDataPair = this.GetLanguagePackage(lang);

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return NotFound();
            }

            User? user = await _userService.GetUserFromToken(token);

            int pagesCount = 1;
            var users = (await _userService.GetUsersList
                              (filter, value, sort, page, _pageSize)).ToList();

            if (!users.IsNullOrEmpty())
            {
                pagesCount = (int)Math.Ceiling(await _userService
                             .GetUsersCount(filter, value, sort) * 1.0 / (_pageSize * 1.0));
            }

            UsersViewModel viewModel = new UsersViewModel()
            {
                User = user,
                LanguagePack = langDataPair,
                Users = users,
                PagesCount = pagesCount,
                CurrentPage = page,
                Filter = filter,
                Sort = sort,
                Value = value,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Promote(string[] userId)
        {
            this.DefineCategories();
            this.SetItemSearch();

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return NotFound();
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null || !user.IsAdmin()) { return BadRequest("You do not have access for this operation"); }

            await _userService.PromoteUsers(userId);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Demote(string[] userId)
        {
            this.DefineCategories();
            this.SetItemSearch();

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return NotFound();
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null || !user.IsAdmin()) { return BadRequest("You do not have access for this operation"); }

            await _userService.DemoteUsers(userId);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] userId)
        {
            this.DefineCategories();
            this.SetItemSearch();

            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return NotFound();
            }

            User? user = await _userService.GetUserFromToken(token);

            if (user == null || !user.IsAdmin()) { return BadRequest("You do not have access for this operation"); }

            await _userService.DeletUsers(userId);

            return Ok();
        }
    }
}
