using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Repositories
{
    public class UserProfileRepository(MovieDbContext dbContext) : BaseRepository<UserProfile>(dbContext), IUserProfileRepository
    {
    }
}
