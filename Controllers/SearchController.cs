using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class SearchModel
    {
        [FromForm]
        public string SearchText { get; set; } = "";
        [FromForm]
        public string CategoryName { get; set; } = "";
        [FromForm]
        public AppLanguage Lang { get; set; } = AppLanguage.en;
        [FromForm]
        public DataSort Sort { get; set; } = DataSort.byDefault;
    }

    public class SearchController : Controller
    {
        private readonly ItemService _itemService;
        private readonly CollectionService _collectionService;

        public SearchController
        (
            [FromServices] ItemService itemService,
            [FromServices] CollectionService collectionService,
            [FromServices] CollectionDBContext dBContext
        )
        {
            _itemService = itemService;
            _collectionService = collectionService;

            _itemService.Initialize(dBContext);
            _collectionService.Initialize(dBContext);
        }

        [HttpPost]
        public IActionResult Item(SearchModel model)
        {
            return RedirectToAction("index", "itemlist", new
            {
                lang = model.Lang.ToString(),
                filter = ItemsDataFilter.bySearch,
                value = model.SearchText,
                sort = model.Sort,
                page = 1,
                categoryName = model.CategoryName
            });
        }

        [HttpPost]
        public IActionResult Collection(SearchModel model)
        {
            return RedirectToAction("index", "collectionList", new
            {
                lang = model.Lang.ToString(),
                filter = CollectionDataFilter.byName,
                value = model.SearchText,
                sort = model.Sort,
                page = 1,
                categoryName = model.CategoryName
            });
        }

        [HttpPost]
        public IActionResult User(SearchModel model)
        {
            return RedirectToAction("index", "users", new
            {
                lang = model.Lang.ToString(),
                filter = UsersDataFilter.byName,
                value = model.SearchText,
                sort = model.Sort,
                page = 1,
            });
        }
    }
}
