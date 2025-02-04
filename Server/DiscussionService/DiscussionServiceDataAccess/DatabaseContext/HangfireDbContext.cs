using Microsoft.EntityFrameworkCore;

namespace DiscussionServiceDataAccess.DatabaseContext
{
    public class HangfireDbContext : DbContext
    {
        public HangfireDbContext() { }
        public HangfireDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
