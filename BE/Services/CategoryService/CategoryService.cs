using BE.Models;
using BE.Models.DTOs;
using BE.Repositories.CategoryRepo;
using Microsoft.EntityFrameworkCore;

namespace BE.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context; // To check for linked products

        public CategoryService(ICategoryRepository categoryRepository, ApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            });
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null) return null;

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            var category = new Category
            {
                Name = createDto.Name
            };

            var created = await _categoryRepository.CreateCategoryAsync(category);
            return new CategoryDto
            {
                CategoryId = created.CategoryId,
                Name = created.Name
            };
        }

        public async Task<bool> UpdateCategoryAsync(int categoryId, CreateCategoryDto updateDto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null) return false;

            category.Name = updateDto.Name;
            await _categoryRepository.UpdateCategoryAsync(category);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null) return false;

            // Check if there are any products under this category
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == categoryId);
            if (hasProducts)
            {
                throw new InvalidOperationException("Không thể xóa danh mục đang chứa sản phẩm.");
            }

            await _categoryRepository.DeleteCategoryAsync(category);
            return true;
        }
    }
}
