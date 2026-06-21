using BE.Models;
using BE.Models.DTOs;
using BE.Repositories.ProductRepo;
using BE.Repositories.CategoryRepo;

namespace BE.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<(IEnumerable<ProductDto> Products, int TotalCount)> GetProductsAsync(
            string? search, 
            int? categoryId, 
            string? sortBy, 
            bool isDescending, 
            int page, 
            int pageSize)
        {
            var (products, totalCount) = await _productRepository.GetProductsAsync(search, categoryId, sortBy, isDescending, page, pageSize);
            
            var productDtos = products.Select(p => MapToDto(p));
            return (productDtos, totalCount);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return null;

            return MapToDto(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            // Validate category existence
            var category = await _categoryRepository.GetCategoryByIdAsync(createDto.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("Danh mục không tồn tại.");
            }

            var product = new Product
            {
                Name = createDto.Name,
                CategoryId = createDto.CategoryId,
                Price = createDto.Price,
                Stock = createDto.Stock,
                InStock = createDto.InStock,
                ShortDescription = createDto.ShortDescription,
                Description = createDto.Description,
                OriginalPrice = createDto.OriginalPrice,
                Sku = createDto.Sku,
                Difficulty = createDto.Difficulty,
                CareLevel = createDto.CareLevel,
                Image = createDto.Image,
                Status = createDto.Status
            };

            var created = await _productRepository.CreateProductAsync(product);
            
            // Reload category reference for mapping
            created.Category = category;

            return MapToDto(created);
        }

        public async Task<bool> UpdateProductAsync(int productId, UpdateProductDto updateDto)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return false;

            // Validate category existence
            var category = await _categoryRepository.GetCategoryByIdAsync(updateDto.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("Danh mục không tồn tại.");
            }

            product.Name = updateDto.Name;
            product.CategoryId = updateDto.CategoryId;
            product.Price = updateDto.Price;
            product.Stock = updateDto.Stock;
            product.InStock = updateDto.InStock;
            product.ShortDescription = updateDto.ShortDescription;
            product.Description = updateDto.Description;
            product.OriginalPrice = updateDto.OriginalPrice;
            product.Sku = updateDto.Sku;
            product.Difficulty = updateDto.Difficulty;
            product.CareLevel = updateDto.CareLevel;
            product.Image = updateDto.Image;
            product.Status = updateDto.Status;

            await _productRepository.UpdateProductAsync(product);
            return true;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return false;

            await _productRepository.DeleteProductAsync(product);
            return true;
        }

        private static ProductDto MapToDto(Product p)
        {
            return new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                Price = p.Price,
                Stock = p.Stock,
                InStock = p.InStock,
                ShortDescription = p.ShortDescription,
                Description = p.Description,
                OriginalPrice = p.OriginalPrice,
                Sku = p.Sku,
                Difficulty = p.Difficulty,
                CareLevel = p.CareLevel,
                Star = p.Star,
                Image = p.Image,
                Status = p.Status
            };
        }
    }
}
