using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Order> Orders, int TotalCount)> GetOrdersAsync(
            string? status,
            int page,
            int pageSize)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                var lowerStatus = status.Trim().ToLower();
                query = query.Where(o => o.Status.ToLower() == lowerStatus);
            }

            int totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalCount);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByOrderCodeAsync(long orderCode)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
