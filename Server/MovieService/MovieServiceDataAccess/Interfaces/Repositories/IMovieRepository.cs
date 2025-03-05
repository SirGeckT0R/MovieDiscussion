using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Interfaces.Repositories
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        Task<Movie?> GetNotApprovedMovieByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ICollection<Movie>> GetAllNotApprovedMoviesAsync(CancellationToken cancellationToken);
    }
}
