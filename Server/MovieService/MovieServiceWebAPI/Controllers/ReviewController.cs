using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApplication.UseCases.Reviews.Commands.AddReviewCommand;
using MovieServiceApplication.UseCases.Reviews.Commands.DeleteReviewCommand;
using MovieServiceApplication.UseCases.Reviews.Commands.UpdateReviewCommand;
using MovieServiceApplication.UseCases.Reviews.Queries.GetReviewByIdQuery;
using MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByMovieIdQuery;
using MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByProfileIdQuery;

namespace MovieServiceWebAPI.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddReviewCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return Created();
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetReviewByIdQuery query, CancellationToken cancellationToken)
        {
            var review = await _mediator.Send(query, cancellationToken);

            return Ok(review);
        }

        [HttpGet("movie/{MovieId:Guid}")]
        public async Task<IActionResult> GetByMovie([FromRoute] GetReviewsByMovieIdQuery query, CancellationToken cancellationToken)
        {
            var review = await _mediator.Send(query, cancellationToken);

            return Ok(review);
        }
        [HttpGet("user/{ProfileId:Guid}")]
        public async Task<IActionResult> GetByUser([FromRoute] GetReviewsByProfileIdQuery query, CancellationToken cancellationToken)
        {
            var review = await _mediator.Send(query, cancellationToken);

            return Ok(review);
        }

        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateReviewCommand command, CancellationToken cancellationToken)
        {
            command.Id = Id;
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteReviewCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
