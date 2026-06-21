using BE.Models.DTOs;
using BE.Services.UserService;
using BE.Services.TokenService;
using BE.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Net;

namespace BE.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        private readonly string _frontendBaseUrl;

        public AuthenticationController(IUserService userService, ITokenService tokenService, IEmailSender emailSender, IConfiguration configuration)
        {
            _userService = userService;
            _tokenService = tokenService;
            _emailSender = emailSender;
            // Đọc FrontendBaseUrl từ config (appsettings/env), fallback về localhost dev
            _frontendBaseUrl = configuration["App:FrontendBaseUrl"] ?? "http://localhost:5254";
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized(new { Message = "Không tìm thấy tài khoản" });
            }

            if (!user.Status)
            {
                return Unauthorized(new { Message = "Tài khoản chưa được kích hoạt. Vui lòng kiểm tra email xác nhận." });
            }

            if(user.PasswordHash == null)
            {
               return BadRequest(new { Message = "Tài khoản này được đăng ký qua Google. Vui lòng chọn đăng nhập bằng Google." });
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Sai tài khoản hoặc mật khẩu" });
            }

            var jwtToken = _tokenService.GenerateToken(user.UserId, user.Role.RoleName, user.FullName, user.Email);


            return Ok(new LoginResponseDto
            {
                JwtToken = jwtToken,
                Role = user.Role.RoleName,
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await  _userService.GetUserByEmailAsync(registerDto.Email);
            if(existingUser != null)
            {
                return Conflict(new { Message = "Email đã được sử dụng" });
            }

            var newUser = await _userService.CreateUserAsync(registerDto);
            var confirmationLink = Url.Action(
                nameof(ConfirmEmail),
                "Authentication",
                new { token = newUser.EmailConfirmationToken },
                Request.Scheme);

            if (string.IsNullOrWhiteSpace(confirmationLink))
            {
                return StatusCode(500, new { Message = "Không tạo được link xác nhận email" });
            }

            try
            {
                await _emailSender.SendEmailAsync(
                    newUser.Email,
                    "Xác nhận tài khoản Vươn",
                    $"""
                    <p>Xin chào {WebUtility.HtmlEncode(newUser.FullName)},</p>
                    <p>Vui lòng bấm vào link bên dưới để kích hoạt tài khoản :</p>
                    <p><a href="{WebUtility.HtmlEncode(confirmationLink)}">Kích hoạt tài khoản</a></p>
                    <p>Link này sẽ hết hạn sau 24 giờ.</p>
                    """);
            }
            catch (Exception ex)
            {
                Console.WriteLine("==================================================");
                Console.WriteLine($"[EMAIL SEND FAILURE] Lỗi gửi email kích hoạt tới {newUser.Email}: {ex.Message}");
                Console.WriteLine($"[ACTIVATION LINK] Link kích hoạt của bạn: {confirmationLink}");
                Console.WriteLine("==================================================");

                // Tự động kích hoạt tài khoản trên môi trường test/demo nếu không gửi được email
                try
                {
                    await _userService.ConfirmEmailAsync(newUser.EmailConfirmationToken ?? "");
                    return Ok(new { Message = "Đăng ký thành công! (Tài khoản đã được TỰ ĐỘNG KÍCH HOẠT do lỗi SMTP gửi mail. Bạn có thể đăng nhập ngay!)" });
                }
                catch (Exception dbEx)
                {
                    Console.WriteLine($"[DB ERROR] Không thể tự động kích hoạt tài khoản: {dbEx.Message}");
                }
            }

            return Ok(new { Message = "Đăng ký thành công. Vui lòng kiểm tra email để kích hoạt tài khoản." });

        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var confirmed = await _userService.ConfirmEmailAsync(token);
            var successMessage = "Kích hoạt tài khoản thành công. Bạn có thể đăng nhập.";
            var failureMessage = "Link xác nhận không hợp lệ hoặc đã hết hạn";
            if (!confirmed)
            {
                var url = $"{_frontendBaseUrl}/login?status=error&message={Uri.EscapeDataString(failureMessage)}";
                return Redirect(url);
            }

            var redirectUrl = $"{_frontendBaseUrl}/login?status=success&message={Uri.EscapeDataString(successMessage)}";

            return Redirect(redirectUrl);
        }

        [HttpGet("oauth/google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties {
                 RedirectUri = Url.Action(nameof(GoogleCallbackComplete)) ?? "/api/Authentication/oauth/google/complete"
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("oauth/google/complete")]
        public async Task<IActionResult> GoogleCallbackComplete()
        {
            var result = await HttpContext.AuthenticateAsync("External");
            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Xác thực google thất bại" });
            }

            var email = result.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var fullName = result.Principal.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new { Message = "Không tìm thấy email" });
            }

            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                var newUser = await _userService.CreateGoogleUserAsync(email, fullName ?? "Google User");
                user = await _userService.GetUserByEmailAsync(newUser.Email);
                if (user == null)
                {
                    return StatusCode(500, new { Message = "Lỗi khi tạo tài khoản từ Google" });
                }
            }
            else
            {
                user = await _userService.ConfirmEmailFromGoogleAsync(email) ?? user;
            }

            if (!user.Status)
            {
                await HttpContext.SignOutAsync("External");
                return Unauthorized(new { Message = "Tài khoản chưa được kích hoạt hoặc đã bị khóa" });
            }

            var jwtToken = _tokenService.GenerateToken(user.UserId, user.Role.RoleName, user.FullName, user.Email);
            await HttpContext.SignOutAsync("External");

            // Dùng _frontendBaseUrl từ config thay vì hardcode localhost
            var redirectUrl = $"{_frontendBaseUrl}/login?token={Uri.EscapeDataString(jwtToken)}&email={Uri.EscapeDataString(user.Email)}&name={Uri.EscapeDataString(user.FullName)}&role={Uri.EscapeDataString(user.Role.RoleName)}";
            return Redirect(redirectUrl);
        }
    }
}
