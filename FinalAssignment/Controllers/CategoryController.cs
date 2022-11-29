using FinalAssignment.DTOs.Asset;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _categoryService.GetAll();
                return new JsonResult(data);
            }

            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest createRequest)
        {

            var category = await _categoryService.GetCategoryByName(createRequest.CategoryName);

            if (category != null)
                return BadRequest("Category is already existed. Please enter a different category. Prefix is already existed. Please enter a different prefix");

            var data = await _categoryService.Create(createRequest);

            if (data == null)
                return StatusCode(500, "Sorry the Request failed");

            return Ok(data);
        }

        [HttpGet("categoryname")]
        public async Task<IActionResult> GetOneAsync(string categoryName)
        {
            var data = await _categoryService.GetCategoryByName(categoryName);

            if (data == null)
                return NotFound();

            return Ok(data);
        }
    }
}
