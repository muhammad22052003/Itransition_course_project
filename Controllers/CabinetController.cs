using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class CabinetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
