using System.ComponentModel.DataAnnotations;

namespace BE.Models.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool InStock { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string? Sku { get; set; }
        public string? Difficulty { get; set; }
        public string? CareLevel { get; set; }
        public decimal? Star { get; set; }
        public string? Image { get; set; }
        public bool Status { get; set; }
    }

    public class CreateProductDto
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Danh mục là bắt buộc.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Giá bán là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải lớn hơn hoặc bằng 0.")]
        public int Stock { get; set; }

        public bool InStock { get; set; } = true;

        [StringLength(500, ErrorMessage = "Mô tả ngắn không được vượt quá 500 ký tự.")]
        public string? ShortDescription { get; set; }

        [StringLength(2000, ErrorMessage = "Mô tả chi tiết không được vượt quá 2000 ký tự.")]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá gốc phải lớn hơn 0.")]
        public decimal? OriginalPrice { get; set; }

        [StringLength(100, ErrorMessage = "Mã SKU không được vượt quá 100 ký tự.")]
        public string? Sku { get; set; }

        [StringLength(50, ErrorMessage = "Độ khó không được vượt quá 50 ký tự.")]
        public string? Difficulty { get; set; }

        [StringLength(250, ErrorMessage = "Mức độ chăm sóc không được vượt quá 250 ký tự.")]
        public string? CareLevel { get; set; }

        public string? Image { get; set; }
        public bool Status { get; set; } = true;
    }

    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Danh mục là bắt buộc.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Giá bán là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải lớn hơn hoặc bằng 0.")]
        public int Stock { get; set; }

        public bool InStock { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả ngắn không được vượt quá 500 ký tự.")]
        public string? ShortDescription { get; set; }

        [StringLength(2000, ErrorMessage = "Mô tả chi tiết không được vượt quá 2000 ký tự.")]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá gốc phải lớn hơn 0.")]
        public decimal? OriginalPrice { get; set; }

        [StringLength(100, ErrorMessage = "Mã SKU không được vượt quá 100 ký tự.")]
        public string? Sku { get; set; }

        [StringLength(50, ErrorMessage = "Độ khó không được vượt quá 50 ký tự.")]
        public string? Difficulty { get; set; }

        [StringLength(250, ErrorMessage = "Mức độ chăm sóc không được vượt quá 250 ký tự.")]
        public string? CareLevel { get; set; }

        public string? Image { get; set; }
        public bool Status { get; set; }
    }
}
