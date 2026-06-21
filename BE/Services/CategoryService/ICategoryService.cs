using BE.Models;
using BE.Models.DTOs;

namespace BE.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);
        Task<bool> UpdateCategoryAsync(int categoryId, CreateCategoryDto updateDto);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
