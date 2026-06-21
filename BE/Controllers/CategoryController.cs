using BE.Models.DTOs;
using BE.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BE.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "Không tìm thấy danh mục" });
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _categoryService.CreateCategoryAsync(createDto);
            return CreatedAtAction(nameof(GetCategory), new { id = created.CategoryId }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _categoryService.UpdateCategoryAsync(id, updateDto);
            if (!result)
            {
                return NotFound(new { Message = "Không tìm thấy danh mục" });
            }

            return Ok(new { Message = "Cập nhật danh mục thành công" });
        }

        [HttpDelete("{id}")]
          [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result)
                {
                    return NotFound(new { Message = "Không tìm thấy danh mục" });
                }

                return Ok(new { Message = "Xóa danh mục thành công" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
