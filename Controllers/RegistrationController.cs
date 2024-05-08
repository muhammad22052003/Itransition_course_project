using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
