using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApplication.UseCases.UserProfiles.Commands.CreateUserProfileCommand;
using MovieServiceApplication.UseCases.UserProfiles.Commands.DeleteUserProfileCommand;
using MovieServiceApplication.UseCases.UserProfiles.Commands.UpdateUserProfileCommand;
using MovieServiceApplication.UseCases.UserProfiles.Queries.GetProfileByAccountIdQuery;

namespace MovieServiceWebAPI.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    public class UserProfileController(IMediator mediator, ILogger<UserProfileController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<UserProfileController> _logger = logger;

        [HttpGet("{AccountId:Guid}")]
        public async Task<IActionResult> GetByAccountId([FromRoute] GetProfileByAccountIdQuery query, CancellationToken cancellationToken)
        {
            var profile = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning profile by account id");

            return Ok(profile);
        }

        [HttpPost("{AccountId:Guid}")]
        public async Task<IActionResult> Create([FromRoute] Guid AccountId, [FromBody] CreateUserProfileCommand command, CancellationToken cancellationToken)
        {
            command.AccountId = AccountId;
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Profile was created");

            return Created();
        }

        [HttpPut("{AccountId:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid AccountId, [FromBody] UpdateUserProfileCommand command, CancellationToken cancellationToken)
        {
            command.AccountId = AccountId;
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Profile was updated");

            return NoContent();
        }

        [HttpDelete("{AccountId:Guid}")]
        public async Task<IActionResult> Update([FromRoute] DeleteUserProfileCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Profile was deleted");

            return NoContent();
        }
    }
}
