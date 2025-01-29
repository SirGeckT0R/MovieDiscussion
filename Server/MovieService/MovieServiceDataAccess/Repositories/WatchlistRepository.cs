using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Repositories
{
    public class WatchlistRepository(MovieDbContext dbContext) : BaseRepository<Watchlist>(dbContext), IWatchlistRepository
    {
    }
}
