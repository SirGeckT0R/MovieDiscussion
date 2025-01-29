using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByAccountIdQuery
{
    public record GetWatchlistByAccountIdQuery(Guid AccountId) : IQuery<WatchlistDto>;
}
