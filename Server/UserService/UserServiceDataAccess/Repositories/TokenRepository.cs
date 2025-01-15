using UserServiceDataAccess.Interfaces.Repositories;
using UserServiceDataAccess.Models;
using UserServiceDataAccess.ServiceDbContext;
using UserServiceDataAccess.Specifications;

namespace UserServiceDataAccess.Repositories
{
    public class TokenRepository(UserServiceDbContext context) : GenericRepository<Token>(context), ITokenRepository
    {
    }
}
