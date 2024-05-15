using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Enums;
using CourseProject_backend.Extensions;
using CourseProject_backend.Models.ViewModels;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CourseProject_backend.Entities;

namespace CourseProject_backend.Controllers
{
    public class CollectionController : Controller
    {
        private readonly int _pageSize = 20;
        private readonly IConfiguration _configuration;
        private readonly ItemService _itemService;
        private readonly CollectionService _collectionService;
        private readonly int pageSize = 20;

        public CollectionController
        (
            [FromServices] IConfiguration configuration,
            [FromServices] ItemService itemService,
            [FromServices] CollectionService collectionService
        )
        {
            _configuration = configuration;
            _itemService = itemService;
            _collectionService = collectionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] AppLanguage lang = AppLanguage.en,
                                               DataSort sort = DataSort.byDefault,
                                               string? id = null,
                                               int page = 1)
        {
            if(id == null) { return NotFound(); }

            this.DefineCategories();

            var langPackSingleton = LanguagePackSingleton.GetInstance();
            var langPackCollection = langPackSingleton.GetLanguagePack(lang);
            if (langPackCollection.IsNullOrEmpty()) { return NotFound(); }

            var langDataPair = new KeyValuePair
                               <string, IDictionary<string, string>>(lang.ToString(), langPackCollection);

            int pagesCount = 1;

            MyCollection? collection = (await _collectionService.GetCollectionList
                (
                    filter: CollectionDataFilter.byId,
                    value: id,
                    DataSort.byDefault,
                    page: 1,
                    pageSize: _pageSize
                )).FirstOrDefault();

            if(collection == null) { return NotFound(); }

            var items = (await _itemService.GetItemsList
                              (
                                filter: ItemsDataFilter.byCollectionId,
                                value: collection.Id,
                                sort: sort,
                                page: page,
                                pageSize: _pageSize)).ToList();

            if (!items.IsNullOrEmpty())
            {
                pagesCount = (int)Math.Ceiling(await _itemService
                             .GetItemsCount
                             (
                                filter: ItemsDataFilter.byCollectionId,
                                value: collection.Id,
                                sort: sort) * 1.0 / (_pageSize * 1.0));
            }

            CollectionViewModel viewModel = new CollectionViewModel()
            {
                LanguagePack = langDataPair,
                Items = items,
                PagesCount = pagesCount,
                CurrentPage = page,
                Sort = sort,
                Collection = collection
            };

            return View(viewModel);
        }
    }
}
