using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieServiceApplication.UseCases.Watchlists.Queries.GetWatchlistByUserIdQuery
{
    public record GetWatchlistByUserIdQuery(Guid UserId) : IQuery<WatchlistDto>;
}
