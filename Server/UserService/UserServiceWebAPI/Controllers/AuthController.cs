using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserServiceApplication.Dto;
using UserServiceApplication.Interfaces.Services;

namespace UserServiceWebAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IUserService userService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            (string accessToken, string refreshToken) = await _userService.LoginAsync(loginRequest, cancellationToken);
            HttpContext.Response.Cookies.Append("accessToken", accessToken, new CookieOptions { Domain = "localhost" });
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions { Domain = "localhost" });

            _logger.LogInformation("User was logged in");

            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest registerUserCommand, CancellationToken cancellationToken)
        {
            (string accessToken, string refreshToken) = await _userService.RegisterAsync(registerUserCommand, cancellationToken);
            HttpContext.Response.Cookies.Append("accessToken", accessToken, new CookieOptions { Domain = "localhost" });
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions { Domain = "localhost" });

            _logger.LogInformation("User has registered");

            return NoContent();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (HttpContext.Request.Cookies["accessToken"] != null)
            {
                HttpContext.Response.Cookies.Append("accessToken", "", new CookieOptions { Domain = "localhost", Expires = DateTime.Now.AddDays(-1) });

                _logger.LogInformation("Access token cleared");
            }

            if (HttpContext.Request.Cookies["refreshToken"] != null)
            {
                HttpContext.Response.Cookies.Append("refreshToken", "", new CookieOptions { Domain = "localhost", Expires = DateTime.Now.AddDays(-1) });

                _logger.LogInformation("Refresh token cleared");
            }

            _logger.LogInformation("User was logged out");

            return NoContent();
        }

        [HttpPost("password/forgot")]
        [Authorize]
        public async Task<IActionResult> ForgotPassword(CancellationToken cancellationToken)
        {
            var accessToken = HttpContext.Request.Cookies["accessToken"];
            var callbackUrl = Url.RouteUrl(
                "ResetPassword",
                values: null,
                protocol: Request.Scheme);

            var token = await _userService.ForgotPasswordAsync(accessToken, callbackUrl!, cancellationToken);

            _logger.LogInformation("Token for resetting password was created");

            return Ok(token);
        }

        [HttpPost("password/reset", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
        {
            await _userService.ResetPasswordAsync(resetPasswordRequest, cancellationToken);

            _logger.LogInformation("User password was reset");

            return Ok("Password was reset");
        }

        [HttpDelete("")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid userId, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(userId, cancellationToken);

            _logger.LogInformation("User was deleted");

            return NoContent();
        }

        [HttpPut("")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateUserRequest updateUserRequest, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(updateUserRequest, cancellationToken);

            _logger.LogInformation("User was updated");

            return NoContent();
        }

        [HttpGet("user")]
        [Authorize(Policy = "User")]
        public IActionResult UserTest()
        {
            return Ok("Only cool Users see this message");
        }

        [HttpGet("admin")]
        [Authorize(Policy = "Admin")]
        public IActionResult AdminTest()
        {
            return Ok("Only cool Admins see this message");
        }

        [HttpGet("role")]
        [Authorize]
        public async Task<IActionResult> GetRole(CancellationToken cancellationToken)
        {
            var accessToken = HttpContext.Request.Cookies["accessToken"];
            var role = await _userService.GetUserRoleByTokenAsync(accessToken, cancellationToken);

            return Ok(role);
        }
    }
}
