using DiscussionServiceApplication.UseCases.Discussions.Commands.ChangeActiveStateOfDiscussionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Commands.CreateDiscussionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Commands.DeleteDiscussionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Commands.SubscribeToDiscussionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Commands.UpdateDiscussionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Queries.GetAllDiscussionsQuery;
using DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionByIdQuery;
using DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionsByCreatorIdQuery;
using DiscussionServiceWebAPI.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionServiceWebAPI.Controllers
{
    [ApiController]
    [Route("/api/discussions")]
    public class DiscussionController(IMediator mediator, ILogger<DiscussionController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<DiscussionController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllDiscussionsQuery();
            var discussions = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning all discussions");

            return Ok(discussions);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDiscussionCommand command, CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var newCommand = command with { CreatedBy = accountId };

            await _mediator.Send(newCommand, cancellationToken);

            _logger.LogInformation("Discussion was created");

            return Created();
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetDiscussionByIdQuery query, CancellationToken cancellationToken)
        {
            var discussion = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning discussion by id");

            return Ok(discussion);
        }

        [HttpPost("{Id:Guid}")]
        public async Task<IActionResult> ChangeActiveState([FromRoute] Guid Id, [FromBody] ChangeActiveStateOfDiscussionCommand command, CancellationToken cancellationToken)
        {
            var newCommand = command with { Id = Id };
            await _mediator.Send(newCommand, cancellationToken);

            _logger.LogInformation("Discussion's state has changed");

            return NoContent();
        }

        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateDiscussionCommand command, CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var newCommand = command with { Id = Id, UpdatedBy = accountId };

            await _mediator.Send(newCommand, cancellationToken);

            _logger.LogInformation("Discussion was updated");

            return NoContent();
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteDiscussionCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Discussion was deleted");

            return NoContent();
        }

        [HttpPost("{Id:Guid}/subscribers")]
        public async Task<IActionResult> Subscribe([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var newCommand = new SubscribeToDiscussionCommand(Id, accountId);

            await _mediator.Send(newCommand, cancellationToken);

            _logger.LogInformation("User was subscribed to a discussion");

            return NoContent();
        }

        [HttpGet("creators/{CreatedBy:Guid}")]
        public async Task<IActionResult> GetAllByCreatorId([FromRoute] GetDiscussionsByCreatorIdQuery query, CancellationToken cancellationToken)
        {
            var discussions = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning all discussions by creator id");

            return Ok(discussions);
        }
    }
}
