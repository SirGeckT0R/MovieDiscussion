using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApplication.UseCases.Movies.Commands.AddMovieCommand;
using MovieServiceApplication.UseCases.Movies.Commands.DeleteMovieCommand;
using MovieServiceApplication.UseCases.Movies.Commands.UpdateMovieCommand;
using MovieServiceApplication.UseCases.Movies.Queries.GetAllMoviesQuery;
using MovieServiceApplication.UseCases.Movies.Queries.GetMovieByIdQuery;

namespace MovieServiceWebAPI.Controllers
{
    [ApiController]
    [Route("/api/movies")]
    public class MovieController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var movies = await _mediator.Send(new GetAllMoviesQuery(), cancellationToken);

            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddMovieCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return Created();
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetMovieByIdQuery query, CancellationToken cancellationToken)
        {
            var movie = await _mediator.Send(query, cancellationToken);

            return Ok(movie);
        }

        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            command.Id = Id;
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteMovieCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
