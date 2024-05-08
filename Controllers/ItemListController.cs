using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class ItemListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
