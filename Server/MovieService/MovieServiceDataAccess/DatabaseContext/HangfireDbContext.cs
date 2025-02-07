using Microsoft.EntityFrameworkCore;

namespace MovieServiceDataAccess.DatabaseContext
{
    public class HangfireDbContext : DbContext
    {
        public HangfireDbContext() { }
        public HangfireDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
