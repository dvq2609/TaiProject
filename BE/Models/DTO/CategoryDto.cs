using System.ComponentModel.DataAnnotations;

namespace BE.Models.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự.")]
        public string Name { get; set; } = null!;
    }
}
