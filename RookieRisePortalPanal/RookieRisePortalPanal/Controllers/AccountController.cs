using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookieRisePortalPanal.Services.AccountService;
using RookieRisePortalPanal.Services.AccountService.DTO;
using System.Security.Claims;

namespace RookieRisePortalPanal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IAccountService accountService ) : ControllerBase
    {


        // 🔐 LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await accountService.LoginAsync(loginDto);

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // 📝 REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            var result = await accountService.RegisterAsync(registerDto);

            return Ok(new
            {
                success = true,
                message = "Registration successful. Check your email to set password.",
                data = result
            });
        }

        // 🔑 SET PASSWORD
        [HttpPost("setpassword")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto dto)
        {
            await accountService.SetPasswordAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Password set successfully"
            });
        }

        // 📧 CHECK EMAIL EXISTS
        [HttpGet("email-exists")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            var exists = await accountService.CheckEmailExistsAsync(email);

            return Ok(new
            {
                success = true,
                data = exists
            });
        }

        // 👤 CURRENT USER
        [Authorize]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid token"
                });

            var user = await accountService.GetCurrentUserAsync(email);

            return Ok(new
            {
                success = true,
                data = user
            });
        }
    }
}
