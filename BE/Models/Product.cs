using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace BE.Models
{
    [Table("Product")]
    [Index(nameof(ProductId), Name = "IX_Product_ProductId", IsUnique = true)]
    
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public Decimal Price { get; set; }
        
        [Required]
        public int Stock { get; set; }

        [Required]
        public bool InStock { get; set; } = true;

        [MaxLength(500)]
        public string? ShortDescription { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        public Decimal? OriginalPrice { get; set; }

        [MaxLength(100)]
        public string? Sku { get; set; }

        [MaxLength(50)]
        public string? Difficulty { get; set; }

        [MaxLength(250)]
        public string? CareLevel { get; set; }

        public decimal? Star { get; set; }
        public string? Image { get; set; }
        [Required]
        public bool Status { get; set; } = true;
        //navigation property
        public virtual Category Category { get; set; } = null!;


    }
}
