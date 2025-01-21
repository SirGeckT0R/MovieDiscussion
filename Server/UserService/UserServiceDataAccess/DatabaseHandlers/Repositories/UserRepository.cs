using Microsoft.EntityFrameworkCore;
using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.Interfaces.Repositories;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.DatabaseHandlers.Repositories
{
    public class UserRepository(UserServiceDbContext dbContext) : GenericRepository<User>(dbContext), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            return user;
        }

        public async Task<User?> GetByEmailTrackingAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _dbSet.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            return user;
        }
    }
}
