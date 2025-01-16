using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.Interfaces.Repositories;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.DatabaseHandlers.Repositories
{
    public class TokenRepository(UserServiceDbContext context) : GenericRepository<Token>(context), ITokenRepository
    {
    }
}
