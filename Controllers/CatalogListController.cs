using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class CatalogListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
