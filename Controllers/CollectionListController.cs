using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Extensions;

namespace CourseProject_backend.Controllers
{
    public class CollectionListController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CollectionService _collectionService;
        private int pageSize = 20;

        public CollectionListController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] CollectionService collectionService
        )
        {
            _configuration = configuration;
            _collectionService = collectionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang,
                                               CollectionDataFilter filter,
                                               string value,
                                               DataSort sort,
                                               int page)
        {
            var langPackSingleton = LanguagePackSingleton.GetInstance();
            var langPackCollection = langPackSingleton.GetLanguagePack(lang);
            if (langPackCollection.IsNullOrEmpty()) { return NotFound(); }

            var langDataPair = new KeyValuePair
                               <string, IDictionary<string, string>>(lang.ToString(), langPackCollection);

            int pagesCount = 1;
            var collections = (await _collectionService
                              .GetCollections(filter, value)).ToList();

            if(!collections.IsNullOrEmpty())
            {
                collections = _collectionService.SortData(collections, sort);
                pagesCount = (int)Math.Ceiling(collections.Count / 20.0);
                collections = collections.GetForPage(page, 20);
            }
            
            CollectionListViewModel viewModel = new CollectionListViewModel()
            {
                LanguagePack = langDataPair,
                Collections = collections,
                PagesCount = pagesCount,
                CurrentPage = page,
                Filter = filter,
                Sort = sort,
                Value = value,
            };

            return View(viewModel);
        }
    }
}
