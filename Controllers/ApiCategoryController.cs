using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CourseProject_backend.Controllers
{
    public class ApiCategoryController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CollectionDBContext _dbContext;

        public ApiCategoryController
        (
            [FromServices]IConfiguration configuration,
            CollectionDBContext dBContext
        )
        {
            _configuration = configuration;
            _dbContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<string> categories = new List<string>();

            categories.Add("All");

            categories.AddRange((await _dbContext.Categories
                .ToListAsync()).Select((x)=>x.Name).ToList());

            categories.Add("Other");

            string json = JsonSerializer.Serialize(categories);

            JsonDocument jsonDocument = JsonDocument.Parse(json);

            return Json(jsonDocument);
        }
    }
}
