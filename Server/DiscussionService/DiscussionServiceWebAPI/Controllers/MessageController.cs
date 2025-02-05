using DiscussionServiceApplication.UseCases.Messages.Queries.GetAllMessagesByDiscussionIdQuery;
using DiscussionServiceApplication.UseCases.Messages.Queries.GetMessageByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionServiceWebAPI.Controllers
{
    [ApiController]
    [Route("/api/messages")]
    public class MessageController(IMediator mediator, ILogger<MessageController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<MessageController> _logger = logger;

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetMessageByIdQuery query, CancellationToken cancellationToken)
        {
            var message = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning message by id");

            return Ok(message);
        }

        [HttpGet("discussions/{DiscussionId:Guid}")]
        public async Task<IActionResult> GetByDiscussionId([FromRoute] GetAllMessagesByDiscussionIdQuery query, CancellationToken cancellationToken)
        {
            var messages = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning all messages by discussion id");

            return Ok(messages);
        }
    }
}
