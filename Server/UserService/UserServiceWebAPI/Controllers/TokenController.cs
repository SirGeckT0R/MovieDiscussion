using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserServiceApplication.Interfaces.Services;

namespace UserServiceWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController(ITokenService tokenService, ILogger<TokenController> logger) : ControllerBase
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly ILogger<TokenController> _logger = logger;

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            string? refreshToken = HttpContext.Request.Cookies["refreshToken"];
            var accessToken = await _tokenService.RefreshTokenAsync(refreshToken, cancellationToken);

            HttpContext.Response.Cookies.Append("accessToken", accessToken, new CookieOptions { Domain = "localhost" });

            _logger.LogInformation("Access token was refreshed");

            return NoContent();
        }
    }
}
