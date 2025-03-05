using Microsoft.EntityFrameworkCore;
using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Repositories
{
    public class MovieRepository(MovieDbContext dbContext) : BaseRepository<Movie>(dbContext), IMovieRepository
    {
        public async Task<Movie?> GetNotApprovedMovieByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var movie = await _dbSet.IgnoreQueryFilters()
                                    .FirstOrDefaultAsync(movie => 
                                                                    movie.Id == id 
                                                                    && !movie.IsApproved
                                                                    && !movie.IsDeleted, 
                                                         cancellationToken
                                                         );

            return movie;
        }

        public async Task<ICollection<Movie>> GetAllNotApprovedMoviesAsync(CancellationToken cancellationToken)
        {
            var movies = await _dbSet.IgnoreQueryFilters()
                                     .Where(movie => 
                                                    !movie.IsApproved
                                                    && !movie.IsDeleted
                                           )
                                     .ToListAsync(cancellationToken);

            return movies;
        }
    }
}
