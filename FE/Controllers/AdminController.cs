using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet("")]
        [HttpGet("index")]
        [HttpGet("index.html")]
        public IActionResult Index() => RedirectToAction("Products");

        [HttpGet("products")]
        [HttpGet("products.html")]
        public IActionResult Products() => View();

        [HttpGet("product-form")]
        [HttpGet("product-form.html")]
        public IActionResult ProductForm() => View();

        [HttpGet("categories")]
        [HttpGet("categories.html")]
        public IActionResult Categories() => View();

        [HttpGet("category-form")]
        [HttpGet("category-form.html")]
        public IActionResult CategoryForm() => View();

        [HttpGet("inventory")]
        [HttpGet("inventory.html")]
        public IActionResult Inventory() => View();

        [HttpGet("orders")]
        [HttpGet("orders.html")]
        public IActionResult Orders() => View();

        [HttpGet("order-detail")]
        [HttpGet("order-detail.html")]
        public IActionResult OrderDetail() => View();

        [HttpGet("update-order-status")]
        [HttpGet("update-order-status.html")]
        public IActionResult UpdateOrderStatus() => View();

        [HttpGet("customers")]
        [HttpGet("customers.html")]
        public IActionResult Customers() => View();

        [HttpGet("customer-detail")]
        [HttpGet("customer-detail.html")]
        public IActionResult CustomerDetail() => View();

        [HttpGet("ai")]
        [HttpGet("ai.html")]
        public IActionResult Ai() => View();

        [HttpGet("faq")]
        [HttpGet("faq.html")]
        public IActionResult Faq() => View();

        [HttpGet("promotions")]
        [HttpGet("promotions.html")]
        public IActionResult Promotions() => View();
    }
}
