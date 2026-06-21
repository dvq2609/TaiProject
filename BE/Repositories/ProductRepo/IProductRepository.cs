using BE.Models;

namespace BE.Repositories.ProductRepo
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsAsync(
            string? search, 
            int? categoryId, 
            string? sortBy, 
            bool isDescending, 
            int page, 
            int pageSize);
            
        Task<Product?> GetProductByIdAsync(int productId);
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
