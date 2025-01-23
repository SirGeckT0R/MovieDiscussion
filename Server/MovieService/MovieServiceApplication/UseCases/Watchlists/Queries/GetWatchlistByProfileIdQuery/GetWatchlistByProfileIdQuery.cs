using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByProfileIdQuery
{
    public record GetWatchlistByProfileIdQuery(Guid ProfileId) : IQuery<WatchlistDto>;
}
