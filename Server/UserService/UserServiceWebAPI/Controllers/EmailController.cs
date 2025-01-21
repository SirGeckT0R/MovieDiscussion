using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserServiceApplication.Dto;
using UserServiceApplication.Interfaces.Services;

namespace UserServiceWebAPI.Controllers
{
    [ApiController]
    public class EmailController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("confirmation/send")]
        [Authorize]
        public async Task<IActionResult> ConfirmEmailSend(CancellationToken cancellationToken)
        {
            var accessToken = HttpContext.Request.Cookies["accessToken"];
            var callbackUrl = Url.RouteUrl(
                "ConfirmEmail",
                values: null,
                protocol: Request.Scheme);

            var token = await _userService.ConfirmEmailSendAsync(accessToken, callbackUrl!, cancellationToken);

            return Ok(token);
        }

        [HttpGet("confirmation", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailRecieve([FromQuery] ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken)
        {
            await _userService.ConfirmEmailRecieveAsync(confirmEmailRequest, cancellationToken);

            return Ok("Email confirmed");
        }
    }
}
