using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDomain.Models;
namespace MovieServiceDataAccess.Repositories
{
    public class GenreRepository(MovieDbContext dbContext) : BaseRepository<Genre>(dbContext), IGenreRepository
    {
        public bool DoExist(ICollection<Guid> genres, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return !genres.Except(_dbSet.Select(g => g.Id)).Any();
        }
    }
}
