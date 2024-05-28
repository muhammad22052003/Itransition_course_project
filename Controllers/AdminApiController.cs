using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Packages;
using CourseProject_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CourseProject_backend.Controllers
{
    public class AdminApiController : Controller
    {
        private readonly CollectionDBContext _dBContext;
        private readonly UserService _userService;

        public AdminApiController
        (
            [FromServices] CollectionDBContext dBContext,
            [FromServices] UserService userService
        )
        {
            _dBContext = dBContext;
            _userService = userService;

            _userService.Initialize(dBContext);
        }

        [HttpGet]
        public async Task<IActionResult> AddCategory(string categoryName)
        {
            
            if (!Request.Cookies.TryGetValue("userData", out string? token))
            {
                return NotFound();
            }

            User? user = await _userService.GetUserFromToken(token);

            if(user != null && user.IsAdmin())
            {
                try
                {
                    int count = await _dBContext.Categories
                                    .Where(c => c.Name.ToLower() == categoryName.ToLower())
                                    .CountAsync();

                    if (count > 0) { return BadRequest("Categoryname already exist"); }

                    Category category = new Category(categoryName, "Category Description");

                    _dBContext.Categories.Add(category);

                    await _dBContext.SaveChangesAsync();

                    await CategoriesPackage.Initialize(_dBContext);

                    var json = new
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description,
                    };

                    try
                    {
                        JsonDocument jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(json));
                        return Ok(json);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Ok();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return NotFound();
        }
    }
}
