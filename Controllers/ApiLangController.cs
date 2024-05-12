using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CourseProject_backend.Controllers
{
    public class ApiLangController : Controller
    {
        private readonly IConfiguration _configuration;

        public ApiLangController
        (
            [FromServices]IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var languages = Enum.GetNames(typeof(AppLanguage));

            string json = JsonSerializer.Serialize(languages);

            JsonDocument jsonDocument = JsonDocument.Parse(json);

            return Json(jsonDocument);
        }
    }
}
