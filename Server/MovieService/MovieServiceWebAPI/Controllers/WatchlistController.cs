using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApplication.UseCases.Watchlists.Commands.CreateWatchlistCommand;
using MovieServiceApplication.UseCases.Watchlists.Commands.DeleteWatchlistCommand;
using MovieServiceApplication.UseCases.Watchlists.Commands.ManageMovieInWatchlistCommand;
using MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByAccountIdQuery;
using MovieServiceWebAPI.Helpers;

namespace MovieServiceWebAPI.Controllers
{
    [ApiController]
    [Route("/api/watchlists")]
    public class WatchlistController(IMediator mediator, ILogger<WatchlistController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<WatchlistController> _logger = logger;

        [HttpPost]
        public async Task<ActionResult> Create(CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var command = new CreateWatchlistCommand(accountId);

            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Watchlist was created");

            return Created();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var command = new DeleteWatchlistCommand(accountId);

            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Watchlist was deleted");

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> ManageMovie([FromBody] ManageMovieInWatchlistCommand command, CancellationToken cancellationToken)
        {
            var accountId = ClaimHelper.GetAccountIdFromUser(HttpContext.User);
            var newCommand = command with { AccountId = accountId };

            await _mediator.Send(newCommand, cancellationToken);

            _logger.LogInformation("Watchlist updated with action: {WatchlistAction}", command.Action);

            return NoContent();
        }

        [HttpGet("{AccountId:Guid}")]
        public async Task<ActionResult> GetByAccountId([FromRoute] GetWatchlistByAccountIdQuery query, CancellationToken cancellationToken)
        {
            var watchlist = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning watchlist by account id");

            return Ok(watchlist);
        }
    }
}
