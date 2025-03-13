using Microsoft.EntityFrameworkCore;
namespace UserServiceDataAccess.DatabaseHandlers.ServiceDbContext
{
    public class HangfireDbContext : DbContext
    {
        public HangfireDbContext() { }
        public HangfireDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
