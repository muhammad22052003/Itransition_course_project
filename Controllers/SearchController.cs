using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Packages;
using CourseProject_backend.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
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
        public IActionResult Item([FromForm]string searchText = "", string categoryName = "", [FromForm]AppLanguage lang = AppLanguage.en)
        {
            return RedirectToAction("index", "itemlist", new
            {
                lang = lang.ToString(),
                filter = ItemsDataFilter.bySearch,
                value = searchText,
                sort = DataSort.byDefault,
                page = 1,
                categoryName = categoryName
            });
        }

        [HttpPost]
        public IActionResult Collection([FromForm] string searchText = "", string categoryName = "", [FromForm] AppLanguage lang = AppLanguage.en)
        {
            return RedirectToAction("index", "collectionList", new
            {
                lang = lang.ToString(),
                filter = CollectionDataFilter.byName,
                value = searchText,
                sort = DataSort.byDefault,
                page = 1,
                categoryName = categoryName
            });
        }

        [HttpPost]
        public IActionResult User([FromForm] string searchText = "", [FromForm] AppLanguage lang = AppLanguage.en)
        {
            return RedirectToAction("index", "users", new
            {
                lang = lang.ToString(),
                filter = UsersDataFilter.byName,
                value = searchText,
                sort = DataSort.byDefault,
                page = 1,
            });
        }
    }
}
