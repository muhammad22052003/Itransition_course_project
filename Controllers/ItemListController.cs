﻿using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Extensions;
using CourseProject_backend.Entities;

namespace CourseProject_backend.Controllers
{
    public class ItemListController : Controller
    {
        private readonly int _pageSize = 12;
        private readonly IConfiguration _configuration;
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly LanguagePackService _languagePackService;

        public ItemListController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] ItemService itemService,
            [FromServices] LanguagePackService languagePackService,
            [FromServices] UserService userService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _languagePackService = languagePackService;
            _configuration = configuration;
            _itemService = itemService;
            _userService = userService;

            _userService.Initialize(dBContext);
            _itemService.Initialize(dBContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang = AppLanguage.en,
                                               ItemsDataFilter filter = ItemsDataFilter.byDefault,
                                               string value = "",
                                               DataSort sort = DataSort.byDefault,
                                               int page = 1,
                                               string categoryName = "")
        {
            this.DefineCategories();
            this.SetItemSearch();
            this.DefineItemsSorts();

            KeyValuePair<string, IDictionary<string, string>> langDataPair = _languagePackService.GetLanguagePackPair(lang);

            int pagesCount = 1;
            var items = (await _itemService.GetItemsList
                              (filter, value, sort, page, _pageSize, categoryName)).ToList();

            if (!items.IsNullOrEmpty())
            {
                pagesCount = (int)Math.Ceiling(await _itemService
                             .GetItemsCount(filter, value, sort, categoryName) * 1.0 / (_pageSize * 1.0));
            }

            string token = string.Empty;
            User? user = null;

            if (Request.Cookies.TryGetValue("userData", out token))
            {
                user = await _userService.GetUserFromToken(token);
            }

            ItemsViewModel viewModel = new ItemsViewModel()
            {
                LanguagePack = langDataPair,
                Items = items,
                User = user,
                PagesCount = pagesCount,
                CurrentPage = page,
                Filter = filter,
                Sort = sort,
                Value = value,
            };

            ViewData.Add("currentCategory", categoryName);
            ViewData.Add("currentSort", sort.ToString());

            return View(viewModel);
        }
    }
}
