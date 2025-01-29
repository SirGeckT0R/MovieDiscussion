using DiscussionServiceApplication.UseCases.Discussions.Commands.CreateDiscussionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Commands.DeleteDiscussionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Commands.UpdateDiscussionCommand;
using DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionByIdQuery;
using DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionsByCreatorIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionServiceWebAPI.Controllers
{
    [ApiController]
    [Route("api/discussions")]
    public class DiscussionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllByCreatorId([FromQuery] GetDiscussionsByCreatorIdQuery query, CancellationToken cancellationToken)
        {
            var discussions = await _mediator.Send(query, cancellationToken);

            return Ok(discussions);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDiscussionCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return Created();
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetDiscussionByIdQuery query, CancellationToken cancellationToken)
        {
            var discussion = await _mediator.Send(query, cancellationToken);

            return Ok(discussion);
        }

        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateDiscussionCommand command, CancellationToken cancellationToken)
        {
            var newCommand = command with { Id = Id };
            await _mediator.Send(newCommand, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] DeleteDiscussionCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
