using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Interfaces.Repositories
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        bool DoExist(ICollection<Guid> genres, CancellationToken cancellationToken);
    }
}
