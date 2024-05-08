using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
