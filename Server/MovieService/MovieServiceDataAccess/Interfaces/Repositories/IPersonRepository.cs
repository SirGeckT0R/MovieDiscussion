using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Interfaces.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        bool DoExist(ICollection<Guid> people, CancellationToken cancellationToken);
    }
}
