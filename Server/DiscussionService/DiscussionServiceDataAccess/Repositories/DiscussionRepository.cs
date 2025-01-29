using DiscussionServiceDataAccess.DatabaseContext;
using DiscussionServiceDataAccess.Interfaces.Repositories;
using DiscussionServiceDomain.Models;

namespace DiscussionServiceDataAccess.Repositories
{
    public class DiscussionRepository(DiscussionDbContext dbContext) : BaseRepository<Discussion>(dbContext), IDiscussionRepository
    {
    }
}
