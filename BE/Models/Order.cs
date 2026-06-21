using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BE.Models
{
    [Table("Order")]
    [Index(nameof(OrderCode), Name = "IX_Order_OrderCode", IsUnique = true)]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public long OrderCode { get; set; } // PayOS order identifier (positive integer)

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; } = 0;

        [MaxLength(500)]
        public string? Note { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReceiverName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ReceiverPhone { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "payos"; // "payos" | "cod"

        [Required]
        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending"; // "Pending" | "Paid" | "Failed" | "Cancelled"

        [MaxLength(200)]
        public string? PaymentLinkId { get; set; } // PayOS payment Link ID

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // "Pending" | "Confirmed" | "Shipping" | "Delivered" | "Cancelled"

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
