using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApplication.UseCases.UserProfiles.Commands.CreateUserProfileCommand;
using MovieServiceApplication.UseCases.UserProfiles.Commands.DeleteUserProfileCommand;
using MovieServiceApplication.UseCases.UserProfiles.Commands.UpdateUserProfileCommand;
using MovieServiceApplication.UseCases.UserProfiles.Queries.GetProfileByAccountIdQuery;
using MovieServiceWebAPI.Helpers;

namespace MovieServiceWebAPI.Controllers
{
    [ApiController]
    [Route("/api/profiles")]
    public class UserProfileController(IMediator mediator, ILogger<UserProfileController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<UserProfileController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var query = new GetProfileByAccountIdQuery(accountId);

            var profile = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning profile");

            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserProfileCommand command, CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var newCommand = command with { AccountId = accountId };

            await _mediator.Send(newCommand, cancellationToken);

            _logger.LogInformation("Profile was created");

            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserProfileCommand command, CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var newCommand = command with { AccountId = accountId };

            await _mediator.Send(newCommand, cancellationToken);

            _logger.LogInformation("Profile was updated");

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var command = new DeleteUserProfileCommand(accountId);

            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Profile was deleted");

            return NoContent();
        }

        [HttpGet("{AccountId:Guid}")]
        public async Task<IActionResult> GetByAccountId([FromRoute] GetProfileByAccountIdQuery query, CancellationToken cancellationToken)
        {
            var profile = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning profile by account id");

            return Ok(profile);
        }
    }
}
