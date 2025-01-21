using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetByEmailTrackingAsync(string email, CancellationToken cancellationToken);
    }
}
