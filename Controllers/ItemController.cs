using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
