using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty; // Snapshot product name at order time

        [MaxLength(500)]
        public string? ProductImage { get; set; } // Snapshot product image at order time

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } // Snapshot price at order time

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; } // Quantity * UnitPrice

        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
