using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class CatalogConstructorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
