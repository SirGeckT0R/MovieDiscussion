using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApplication.UseCases.Watchlists.Commands.CreateWatchlistCommand;
using MovieServiceApplication.UseCases.Watchlists.Commands.DeleteWatchlistCommand;
using MovieServiceApplication.UseCases.Watchlists.Commands.ManageMovieInWatchlistCommand;
using MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByAccountIdQuery;

namespace MovieServiceWebAPI.Controllers
{
    [ApiController]
    [Route("api/watchlists")]
    public class WatchlistController(IMediator mediator, ILogger<WatchlistController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<WatchlistController> _logger = logger;

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateWatchlistCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Watchlist was created");

            return Created();
        }

        [HttpGet("{AccountId:Guid}")]
        public async Task<ActionResult> GetByUserId([FromRoute] GetWatchlistByAccountIdQuery query,CancellationToken cancellationToken)
        {
            var watchlist = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning watchlist by id");

            return Ok(watchlist);
        }

        [HttpDelete("{AccountId:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] DeleteWatchlistCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Watchlist was deleted");

            return NoContent();
        }

        [HttpPost("{AccountId:Guid}")]
        public async Task<ActionResult> ManageMovie([FromRoute] Guid AccountId, [FromBody] ManageMovieInWatchlistCommand command, CancellationToken cancellationToken)
        {
            command.AccountId = AccountId;
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Watchlist updated with action: {WatchlistAction}", command.Action);

            return NoContent();
        }
    }
}
