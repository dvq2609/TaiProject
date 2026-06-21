using BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Controllers
{
    [Route("api/admin/dashboard")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                // Tính toán theo múi giờ Việt Nam (UTC+7) để thống kê chính xác ngày trên server UTC
                var utcNow = DateTime.UtcNow;
                var vietnamNow = utcNow.AddHours(7);
                var vietnamToday = vietnamNow.Date;
                var todayUtc = vietnamToday.AddHours(-7); // 00:00:00 ngày hôm nay ở VN, quy về UTC

                var vietnamStartOfMonth = new DateTime(vietnamNow.Year, vietnamNow.Month, 1);
                var startOfMonthUtc = vietnamStartOfMonth.AddHours(-7); // 00:00:00 ngày đầu tháng ở VN, quy về UTC

                // 1. Doanh thu hôm nay: các đơn đã thanh toán hoặc đơn COD đã hoàn thành (Delivered)
                var todayRevenue = await _context.Orders
                    .Where(o => o.CreatedAt >= todayUtc && 
                                (o.PaymentStatus == "Paid" || (o.PaymentMethod == "cod" && o.Status == "Delivered")))
                    .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

                // 2. Doanh thu cả tháng: các đơn tương tự từ đầu tháng tới giờ
                var monthRevenue = await _context.Orders
                    .Where(o => o.CreatedAt >= startOfMonthUtc && 
                                (o.PaymentStatus == "Paid" || (o.PaymentMethod == "cod" && o.Status == "Delivered")))
                    .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

                // 3. Số lượng khách hàng (tài khoản có RoleId = 2)
                var customerCount = await _context.Users
                    .CountAsync(u => u.RoleId == 2);

                // 4. Số lượng đơn hàng thành công: đơn đã thanh toán hoặc đơn COD không bị hủy/không ở trạng thái chờ xác nhận
                var successfulOrderCount = await _context.Orders
                    .CountAsync(o => o.PaymentStatus == "Paid" || 
                                     (o.PaymentMethod == "cod" && o.Status != "Cancelled" && o.Status != "Pending"));

                return Ok(new
                {
                    TodayRevenue = todayRevenue,
                    MonthRevenue = monthRevenue,
                    CustomerCount = customerCount,
                    SuccessfulOrderCount = successfulOrderCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }
    }
}
