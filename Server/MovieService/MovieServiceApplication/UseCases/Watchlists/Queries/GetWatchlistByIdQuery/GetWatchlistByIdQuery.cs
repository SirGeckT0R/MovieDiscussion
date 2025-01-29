using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByIdQuery
{
    public record GetWatchlistByIdQuery(Guid Id) : IQuery<WatchlistDto>;
}
