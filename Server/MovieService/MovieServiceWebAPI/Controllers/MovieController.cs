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
    public class MovieController(IMediator mediator, ILogger<MovieController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<MovieController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var movies = await _mediator.Send(new GetAllMoviesQuery(), cancellationToken);

            _logger.LogInformation("Returning all movies");

            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddMovieCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Movie was created");

            return Created();
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetMovieByIdQuery query, CancellationToken cancellationToken)
        {
            var movie = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning movie by id");

            return Ok(movie);
        }

        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            command.Id = Id;
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Movie was updated");

            return NoContent();
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteMovieCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Movie was deleted");

            return NoContent();
        }
    }
}
