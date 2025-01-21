using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApplication.UseCases.Genres.Commands.AddGenreCommand;
using MovieServiceApplication.UseCases.Genres.Commands.DeleteGenreCommand;
using MovieServiceApplication.UseCases.Genres.Commands.UpdateGenreCommand;
using MovieServiceApplication.UseCases.Genres.Queries.GetAllGenresQuery;
using MovieServiceApplication.UseCases.Genres.Queries.GetGenreByIdQuery;

namespace MovieServiceWebAPI
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController(IMediator mediator): ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var genres = await _mediator.Send(new GetAllGenresQuery(), cancellationToken);

            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AddGenreCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return Created();
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetGenreByIdQuery query, CancellationToken cancellationToken)
        {
            var genre = await _mediator.Send(query, cancellationToken);

            return Ok(genre);
        }

        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromForm] UpdateGenreCommand command, CancellationToken cancellationToken)
        {
            command.Id = Id;
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteGenreCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

    }
}
