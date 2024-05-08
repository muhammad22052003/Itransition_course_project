using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
