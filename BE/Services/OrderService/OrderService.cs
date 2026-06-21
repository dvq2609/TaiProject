using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using BE.Models.DTOs;
using BE.Repositories.OrderRepo;
using BE.Repositories.ProductRepo;
using Microsoft.Extensions.Configuration;
using PayOS;
using PayOS.Models;
using PayOS.Models.Webhooks;
using PayOS.Models.V2.PaymentRequests;

namespace BE.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly PayOSClient _payOSClient;
        private readonly IConfiguration _configuration;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            PayOSClient payOSClient,
            IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _payOSClient = payOSClient;
            _configuration = configuration;
        }

        public async Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto createDto)
        {
            // 1. Lấy danh sách sản phẩm từ DB và validate stock
            var orderItems = new List<OrderItem>();
            decimal totalProductAmount = 0;

            foreach (var itemDto in createDto.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(itemDto.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Sản phẩm có ID {itemDto.ProductId} không tồn tại.");
                }

                if (product.Stock < itemDto.Quantity)
                {
                    throw new InvalidOperationException($"Sản phẩm '{product.Name}' không đủ số lượng trong kho. Hiện tại còn {product.Stock} sản phẩm.");
                }

                var unitPrice = product.Price;
                var subtotal = unitPrice * itemDto.Quantity;
                totalProductAmount += subtotal;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    ProductImage = product.Image,
                    UnitPrice = unitPrice,
                    Quantity = itemDto.Quantity,
                    Subtotal = subtotal
                });
            }

            // 2. Tính phí vận chuyển (ví dụ: free ship cho đơn từ 500.000đ, ngược lại 30.000đ)
            decimal shippingFee = totalProductAmount >= 500000 ? 0 : 30000;
            decimal totalAmount = totalProductAmount + shippingFee;

            // 3. Tạo mã OrderCode (PayOS yêu cầu số nguyên dương độc nhất)
            long orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var order = new Order
            {
                UserId = userId,
                OrderCode = orderCode,
                TotalAmount = totalAmount,
                ShippingFee = shippingFee,
                Note = createDto.Note,
                ReceiverName = createDto.ReceiverName,
                ReceiverPhone = createDto.ReceiverPhone,
                ShippingAddress = createDto.ShippingAddress,
                PaymentMethod = createDto.PaymentMethod.ToLower(),
                PaymentStatus = "Pending",
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                OrderItems = orderItems
            };

            // 4. Trừ số lượng kho
            foreach (var itemDto in createDto.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(itemDto.ProductId);
                if (product != null)
                {
                    product.Stock -= itemDto.Quantity;
                    product.InStock = product.Stock > 0;
                    await _productRepository.UpdateProductAsync(product);
                }
            }

            // 5. Nếu dùng PayOS -> tạo link thanh toán
            string? checkoutUrl = null;
            string? paymentLinkId = null;

            if (order.PaymentMethod == "payos")
            {
                try
                {
                    var payOSItems = order.OrderItems.Select(oi => new PaymentLinkItem { Name = oi.ProductName, Quantity = oi.Quantity, Price = (long)oi.UnitPrice }).ToList();
                    if (shippingFee > 0)
                    {
                        payOSItems.Add(new PaymentLinkItem { Name = "Phí vận chuyển", Quantity = 1, Price = (long)shippingFee });
                    }

                    var returnUrl = _configuration["PayOS:ReturnUrl"] ?? "http://localhost:5254/payment-success";
                    var cancelUrl = _configuration["PayOS:CancelUrl"] ?? "http://localhost:5254/payment-cancel";

                    var paymentRequest = new CreatePaymentLinkRequest
                    {
                        OrderCode = orderCode,
                        Amount = (long)totalAmount,
                        Description = $"DH {orderCode}",
                        Items = payOSItems,
                        ReturnUrl = returnUrl,
                        CancelUrl = cancelUrl
                    };

                    var paymentLinkResult = await _payOSClient.PaymentRequests.CreateAsync(paymentRequest);
                    checkoutUrl = paymentLinkResult.CheckoutUrl;
                    paymentLinkId = paymentLinkResult.PaymentLinkId;

                    // Lưu PaymentLinkId vào order
                    order.PaymentLinkId = paymentLinkId;
                }
                catch (Exception ex)
                {
                    // Nếu tạo link PayOS fail -> rollback stock và rethrow
                    foreach (var itemDto in createDto.Items)
                    {
                        var product = await _productRepository.GetProductByIdAsync(itemDto.ProductId);
                        if (product != null)
                        {
                            product.Stock += itemDto.Quantity;
                            product.InStock = product.Stock > 0;
                            await _productRepository.UpdateProductAsync(product);
                        }
                    }
                    throw new InvalidOperationException($"Lỗi kết nối cổng thanh toán PayOS: {ex.Message}", ex);
                }
            }

            // 6. Lưu đơn hàng vào DB
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            var orderDto = MapToDto(createdOrder);
            orderDto.CheckoutUrl = checkoutUrl;

            return orderDto;
        }

        public async Task<(IEnumerable<OrderDto> Orders, int TotalCount)> GetOrdersAsync(string? status, int page, int pageSize)
        {
            var (orders, totalCount) = await _orderRepository.GetOrdersAsync(status, page, pageSize);
            var orderDtos = orders.Select(o => MapToDto(o)).ToList();
            return (orderDtos, totalCount);
        }

        public async Task<IEnumerable<OrderDto>> GetMyOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(o => MapToDto(o)).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId, int currentUserId, string currentUserRole)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return null;

            // Kiểm tra quyền: Chỉ admin hoặc chính chủ đơn hàng mới được xem
            if (currentUserRole.ToLower() != "admin" && order.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xem thông tin đơn hàng này.");
            }

            return MapToDto(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }

        public async Task<bool> CancelOrderAsync(int orderId, int currentUserId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            // Khách chỉ được hủy đơn của mình và khi đơn ở trạng thái Pending
            if (order.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền hủy đơn hàng này.");
            }

            if (order.Status.ToLower() != "pending")
            {
                throw new InvalidOperationException("Đơn hàng đã được xử lý, không thể tự hủy.");
            }

            order.Status = "Cancelled";
            order.PaymentStatus = "Cancelled";
            order.UpdatedAt = DateTime.UtcNow;

            // Hoàn lại số lượng kho
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                    product.InStock = product.Stock > 0;
                    await _productRepository.UpdateProductAsync(product);
                }
            }

            // Gọi PayOS để hủy link thanh toán nếu có
            if (order.PaymentMethod == "payos" && !string.IsNullOrEmpty(order.PaymentLinkId))
            {
                try
                {
                    await _payOSClient.PaymentRequests.CancelAsync(order.OrderCode, "Khách hàng tự hủy đơn");
                }
                catch (Exception)
                {
                    // Lỗi hủy link PayOS có thể bỏ qua hoặc log lại, không block flow chính của khách
                }
            }

            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }

        public async Task<bool> HandlePayOSWebhookAsync(WebhookData webhookData)
        {
            // Verify webhook signature (SDK tự động verify khi gọi Webhooks.VerifyAsync trong controller,
            // hoặc ta verify ở đây. Để giữ cấu trúc phân lớp, controller sẽ verify rồi truyền data vào đây).
            var order = await _orderRepository.GetOrderByOrderCodeAsync(webhookData.OrderCode);
            if (order == null)
            {
                // Nếu không tìm thấy đơn hàng (ví dụ: request TEST từ PayOS có orderCode = 0),
                // ta trả về true để controller phản hồi 200 OK giúp đăng ký webhook thành công.
                return true;
            }

            // Nếu đơn hàng đã xử lý rồi (thành công hoặc đã hủy) thì bỏ qua
            if (order.PaymentStatus == "Paid" || order.Status == "Cancelled")
            {
                return true;
            }

            // Cập nhật trạng thái dựa vào code từ PayOS webhook
            // Mã "00" là thành công
            if (webhookData.Code == "00")
            {
                order.PaymentStatus = "Paid";
                order.Status = "Confirmed";
            }
            else
            {
                // Thất bại
                order.PaymentStatus = "Failed";
                order.Status = "Cancelled";

                // Hoàn lại kho nếu thất bại
                foreach (var item in order.OrderItems)
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                        product.InStock = product.Stock > 0;
                        await _productRepository.UpdateProductAsync(product);
                    }
                }
            }

            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }

        private static OrderDto MapToDto(Order o)
        {
            return new OrderDto
            {
                OrderId = o.OrderId,
                OrderCode = o.OrderCode,
                Status = o.Status,
                PaymentStatus = o.PaymentStatus,
                PaymentMethod = o.PaymentMethod,
                TotalAmount = o.TotalAmount,
                ShippingFee = o.ShippingFee,
                Note = o.Note,
                ReceiverName = o.ReceiverName,
                ReceiverPhone = o.ReceiverPhone,
                ShippingAddress = o.ShippingAddress,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    ProductImage = oi.ProductImage,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    Subtotal = oi.Subtotal
                }).ToList()
            };
        }
    }
}
