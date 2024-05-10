using Microsoft.AspNetCore.Mvc;

namespace CourseProject_backend.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration _configuration;

        public LoginController
        (
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        public IActionResult Index
        (
            IConfiguration configuration
        )
        {
            return View();
        }
    }
}
