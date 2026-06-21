using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models.DTOs;
using PayOS.Models.Webhooks;

namespace BE.Services.OrderService
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto createDto);
        Task<(IEnumerable<OrderDto> Orders, int TotalCount)> GetOrdersAsync(string? status, int page, int pageSize);
        Task<IEnumerable<OrderDto>> GetMyOrdersAsync(int userId);
        Task<OrderDto?> GetOrderByIdAsync(int orderId, int currentUserId, string currentUserRole);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> CancelOrderAsync(int orderId, int currentUserId);
        Task<bool> HandlePayOSWebhookAsync(WebhookData webhookData);
    }
}
