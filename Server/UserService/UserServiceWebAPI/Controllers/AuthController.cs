using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserServiceApplication.Dto;
using UserServiceApplication.Interfaces.Services;

namespace UserServiceWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            (string accessToken, string refreshToken) = await _userService.LoginAsync(loginRequest, cancellationToken);
            HttpContext.Response.Cookies.Append("accessToken", accessToken, new CookieOptions { Domain = "localhost" });
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions { Domain = "localhost" });
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest registerUserCommand, CancellationToken cancellationToken)
        {
            (string accessToken, string refreshToken) = await _userService.RegisterAsync(registerUserCommand, cancellationToken);
            HttpContext.Response.Cookies.Append("accessToken", accessToken, new CookieOptions { Domain = "localhost" });
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions { Domain = "localhost" });
            return NoContent();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            if (HttpContext.Request.Cookies["accessToken"] != null)
            {
                HttpContext.Response.Cookies.Append("accessToken", "", new CookieOptions { Domain = "localhost", Expires = DateTime.Now.AddDays(-1) });
            }

            if (HttpContext.Request.Cookies["refreshToken"] != null)
            {
                HttpContext.Response.Cookies.Append("refreshToken", "", new CookieOptions { Domain = "localhost", Expires = DateTime.Now.AddDays(-1) });
            }
            return NoContent();
        }

        [HttpGet("confirm")]
        [Authorize]
        public async Task<IActionResult>ConfirmEmailSend(CancellationToken cancellationToken)
        {
            var accessToken = HttpContext.Request.Cookies["accessToken"];
            var callbackUrl = Url.RouteUrl(
                "ConfirmEmail",
                values: null,
                protocol: Request.Scheme);

            var token =await _userService.ConfirmEmailSendAsync(accessToken, callbackUrl!, cancellationToken);
            return Ok(token);
        }

        [HttpGet("confirmRecieve", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailRecieve([FromQuery] ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken)
        {
            await _userService.ConfirmEmailRecieveAsync(confirmEmailRequest, cancellationToken);
            return Ok("Email confirmed");
        }

        [HttpDelete("")]
        public async Task<IActionResult> Delete(Guid userId, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(userId, cancellationToken);
            return NoContent();
        }

        [HttpPut("")]
        public async Task<IActionResult> Update([FromForm] UpdateUserRequest updateUserRequest, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(updateUserRequest, cancellationToken);
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
    }
}
