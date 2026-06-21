using BE.Models;

namespace BE.Repositories.CategoryRepo
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int categoryId);
        Task<Category> CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
