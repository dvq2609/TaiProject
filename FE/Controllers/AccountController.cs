using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("login")]
        [HttpGet("login.html")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        [HttpGet("register.html")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("forgot-password")]
        [HttpGet("forgot-password.html")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet("verify-otp")]
        [HttpGet("verify-otp.html")]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpGet("change-password")]
        [HttpGet("change-password.html")]
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
