using UserServiceDataAccess.Interfaces.Repositories;
using UserServiceDataAccess.Models;
using UserServiceDataAccess.ServiceDbContext;

namespace UserServiceDataAccess.Repositories
{
    public class TokenRepository(UserServiceDbContext context) : GenericRepository<Token>(context), ITokenRepository
    {
    }
}
