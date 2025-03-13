using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserServiceApplication.Dto;
using UserServiceApplication.Interfaces.Services;

namespace UserServiceWebAPI.Controllers
{
    [ApiController]
    public class EmailController(IUserService userService, IConfiguration configuration, ILogger<EmailController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<EmailController> _logger = logger;

        [HttpPost("confirmation/send")]
        [Authorize]
        public async Task<IActionResult> ConfirmEmailSend(CancellationToken cancellationToken)
        {
            var accessToken = HttpContext.Request.Cookies["accessToken"];
            
            var callbackUrl = $"{_configuration["FrontEndUrl"]}/confirmation";

            var token = await _userService.ConfirmEmailSendAsync(accessToken, callbackUrl!, cancellationToken);

            _logger.LogInformation("Confirmation email sent to user");

            return Ok(token);
        }

        [HttpGet("confirmation", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailRecieve([FromQuery] ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken)
        {
            await _userService.ConfirmEmailRecieveAsync(confirmEmailRequest, cancellationToken);

            _logger.LogInformation("User email confirmed");

            return Ok("Email confirmed");
        }
    }
}
