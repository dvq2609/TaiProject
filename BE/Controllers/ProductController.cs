using BE.Models.DTOs;
using BE.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] string? search,
            [FromQuery] int? categoryId,
            [FromQuery] string? sortBy,
            [FromQuery] bool isDescending = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Limit page size to prevent memory issues

            var (products, totalCount) = await _productService.GetProductsAsync(search, categoryId, sortBy, isDescending, page, pageSize);

            return Ok(new
            {
                Products = products,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Không tìm thấy sản phẩm" });
            }
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _productService.CreateProductAsync(createDto);
                return CreatedAtAction(nameof(GetProduct), new { id = created.ProductId }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _productService.UpdateProductAsync(id, updateDto);
                if (!result)
                {
                    return NotFound(new { Message = "Không tìm thấy sản phẩm" });
                }

                return Ok(new { Message = "Cập nhật sản phẩm thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Không tìm thấy sản phẩm" });
            }

            return Ok(new { Message = "Xóa sản phẩm thành công" });
        }
    }
}
