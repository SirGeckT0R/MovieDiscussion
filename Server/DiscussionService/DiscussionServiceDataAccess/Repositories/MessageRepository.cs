using DiscussionServiceDataAccess.DatabaseContext;
using DiscussionServiceDataAccess.Interfaces.Repositories;
using DiscussionServiceDomain.Models;

namespace DiscussionServiceDataAccess.Repositories
{
    public class MessageRepository(DiscussionDbContext dbContext) : BaseRepository<Message>(dbContext), IMessageRepository
    {
    }
}
