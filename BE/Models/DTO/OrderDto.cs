using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BE.Models.DTOs
{
    public class CreateOrderItemDto
    {
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
        [Range(1, 1000, ErrorMessage = "Số lượng sản phẩm phải từ 1 đến 1000.")]
        public int Quantity { get; set; }
    }

    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Tên người nhận là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên người nhận không được vượt quá 100 ký tự.")]
        public string ReceiverName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng.")]
        public string ReceiverPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ nhận hàng là bắt buộc.")]
        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự.")]
        public string ShippingAddress { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Phương thức thanh toán là bắt buộc.")]
        [RegularExpression("^(payos|cod)$", ErrorMessage = "Phương thức thanh toán phải là 'payos' hoặc 'cod'.")]
        public string PaymentMethod { get; set; } = "payos";

        [Required(ErrorMessage = "Danh sách sản phẩm không được trống.")]
        [MinLength(1, ErrorMessage = "Phải có ít nhất 1 sản phẩm trong đơn hàng.")]
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class OrderDto
    {
        public int OrderId { get; set; }
        public long OrderCode { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public string? Note { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public string? CheckoutUrl { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "Trạng thái đơn hàng là bắt buộc.")]
        [RegularExpression("^(Pending|Confirmed|Shipping|Delivered|Cancelled)$", ErrorMessage = "Trạng thái đơn hàng không hợp lệ.")]
        public string Status { get; set; } = string.Empty;
    }
}
