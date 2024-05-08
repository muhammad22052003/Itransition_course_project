using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
