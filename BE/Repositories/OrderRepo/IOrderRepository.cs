using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Repositories.OrderRepo
{
    public interface IOrderRepository
    {
        Task<(IEnumerable<Order> Orders, int TotalCount)> GetOrdersAsync(
            string? status,
            int page,
            int pageSize);

        Task<Order?> GetOrderByIdAsync(int orderId);

        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);

        Task<Order?> GetOrderByOrderCodeAsync(long orderCode);

        Task<Order> CreateOrderAsync(Order order);

        Task UpdateOrderAsync(Order order);
    }
}
