using MongoDB.Driver;
using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Repositories
{
    public class PersonRepository(MovieDbContext dbContext) : BaseRepository<Person>(dbContext), IPersonRepository
    {
        public bool DoExist(ICollection<Guid> people, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return !people.Except(_dbSet.Select(x => x.Id)).Any();
        }
    }
}
