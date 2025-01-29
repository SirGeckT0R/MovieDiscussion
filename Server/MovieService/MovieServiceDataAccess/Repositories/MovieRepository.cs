using MongoDB.Driver;
using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDomain.Models;
namespace MovieServiceDataAccess.Repositories
{
    public class MovieRepository(MovieDbContext dbContext) : BaseRepository<Movie>(dbContext), IMovieRepository
    {
    }
}
