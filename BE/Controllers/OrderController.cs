using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BE.Models.DTOs;
using BE.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayOS;
using PayOS.Models;
using PayOS.Models.Webhooks;

namespace BE.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly PayOSClient _payOSClient;

        public OrderController(IOrderService orderService, PayOSClient payOSClient)
        {
            _orderService = orderService;
            _payOSClient = payOSClient;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { Message = "Không tìm thấy thông tin tài khoản." });
            }

            try
            {
                var orderDto = await _orderService.CreateOrderAsync(userId, createDto);
                return CreatedAtAction(nameof(GetOrder), new { id = orderDto.OrderId }, orderDto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyOrders()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { Message = "Không tìm thấy thông tin tài khoản." });
            }

            var orders = await _orderService.GetMyOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrder(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { Message = "Không tìm thấy thông tin tài khoản." });
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            try
            {
                var order = await _orderService.GetOrderByIdAsync(id, userId, userRole);
                if (order == null)
                {
                    return NotFound(new { Message = "Không tìm thấy đơn hàng." });
                }

                return Ok(order);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpPost("{id}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { Message = "Không tìm thấy thông tin tài khoản." });
            }

            try
            {
                var result = await _orderService.CancelOrderAsync(id, userId);
                if (!result)
                {
                    return NotFound(new { Message = "Không tìm thấy đơn hàng." });
                }

                return Ok(new { Message = "Hủy đơn hàng thành công." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Lỗi khi hủy đơn hàng: {ex.Message}" });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] Webhook webhookBody)
        {
            try
            {
                // Verify webhook signature
                var verifiedData = await _payOSClient.Webhooks.VerifyAsync(webhookBody);

                // Update order in database
                var result = await _orderService.HandlePayOSWebhookAsync(verifiedData);
                if (!result)
                {
                    return NotFound(new { Message = "Đơn hàng không tồn tại hoặc đã được xử lý." });
                }

                return Ok(new { Message = "Cập nhật đơn hàng thành công từ cổng thanh toán." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Webhook signature verification failed: {ex.Message}" });
            }
        }

        // --- ADMIN API ---

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders(
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (orders, totalCount) = await _orderService.GetOrdersAsync(status, page, pageSize);

            return Ok(new
            {
                Orders = orders,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.UpdateOrderStatusAsync(id, updateDto.Status);
            if (!result)
            {
                return NotFound(new { Message = "Không tìm thấy đơn hàng." });
            }

            return Ok(new { Message = "Cập nhật trạng thái đơn hàng thành công." });
        }
    }
}
