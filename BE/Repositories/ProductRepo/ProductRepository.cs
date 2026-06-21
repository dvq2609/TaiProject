using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsAsync(
            string? search, 
            int? categoryId, 
            string? sortBy, 
            bool isDescending, 
            int page, 
            int pageSize)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            // Search by Name or Sku (case-insensitive)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(lowerSearch) || 
                                         (p.Sku != null && p.Sku.ToLower().Contains(lowerSearch)));
            }

            // Category Filter
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // Total Count before pagination
            int totalCount = await query.CountAsync();

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "price":
                        query = isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                        break;
                    case "name":
                        query = isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                        break;
                    case "id":
                    default:
                        query = isDescending ? query.OrderByDescending(p => p.ProductId) : query.OrderBy(p => p.ProductId);
                        break;
                }
            }
            else
            {
                // Default sorting by ProductId descending to show newest first
                query = query.OrderByDescending(p => p.ProductId);
            }

            // Pagination
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
