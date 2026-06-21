using BE.Models.DTOs;

namespace BE.Services.ProductService
{
    public interface IProductService
    {
        Task<(IEnumerable<ProductDto> Products, int TotalCount)> GetProductsAsync(
            string? search, 
            int? categoryId, 
            string? sortBy, 
            bool isDescending, 
            int page, 
            int pageSize);
            
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
        Task<bool> UpdateProductAsync(int productId, UpdateProductDto updateDto);
        Task<bool> DeleteProductAsync(int productId);
    }
}
