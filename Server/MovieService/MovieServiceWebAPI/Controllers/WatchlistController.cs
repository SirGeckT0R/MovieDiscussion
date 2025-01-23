using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApplication.UseCases.Watchlists.Commands.CreateWatchlistCommand;
using MovieServiceApplication.UseCases.Watchlists.Commands.DeleteWatchlistCommand;
using MovieServiceApplication.UseCases.Watchlists.Commands.ManageMovieInWatchlistCommand;
using MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByProfileIdQuery;

namespace MovieServiceWebAPI.Controllers
{
    [ApiController]
    [Route("api/watchlists")]
    public class WatchlistController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateWatchlistCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return Created();
        }

        [HttpGet("{ProfileId:Guid}")]
        public async Task<ActionResult> GetByUserId([FromRoute] GetWatchlistByProfileIdQuery query,CancellationToken cancellationToken)
        {
            var watchlist = await _mediator.Send(query, cancellationToken);

            return Ok(watchlist);
        }

        [HttpDelete("{ProfileId:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] DeleteWatchlistCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("{ProfileId:Guid}")]
        public async Task<ActionResult> ManageMovie([FromRoute] Guid ProfileId, [FromBody] ManageMovieInWatchlistCommand command, CancellationToken cancellationToken)
        {
            command.ProfileId = ProfileId;
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
