using FE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FE.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        [HttpGet("home")]
        [HttpGet("home.html")]
        [HttpGet("index")]
        [HttpGet("index.html")]
        public IActionResult Index() => View();

        [HttpGet("cart")]
        [HttpGet("cart.html")]
        public IActionResult Cart() => View();

        [HttpGet("checkout-shipping")]
        [HttpGet("checkout-shipping.html")]
        public IActionResult CheckoutShipping() => View();

        [HttpGet("checkout-payment")]
        [HttpGet("checkout-payment.html")]
        public IActionResult CheckoutPayment() => View();

        [HttpGet("checkout-confirm")]
        [HttpGet("checkout-confirm.html")]
        public IActionResult CheckoutConfirm() => View();

        [HttpGet("orders")]
        [HttpGet("orders.html")]
        public IActionResult Orders() => View();

        [HttpGet("payment-success")]
        [HttpGet("payment-success.html")]
        public IActionResult PaymentSuccess() => View();

        [HttpGet("payment-cancel")]
        [HttpGet("payment-cancel.html")]
        public IActionResult PaymentCancel() => View();

        [HttpGet("order-success")]
        [HttpGet("order-success.html")]
        public IActionResult OrderSuccess() => View();

        [HttpGet("track-order")]
        [HttpGet("track-order.html")]
        public IActionResult TrackOrder() => View();

        [HttpGet("product-detail")]
        [HttpGet("product-detail.html")]
        public IActionResult ProductDetail() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
