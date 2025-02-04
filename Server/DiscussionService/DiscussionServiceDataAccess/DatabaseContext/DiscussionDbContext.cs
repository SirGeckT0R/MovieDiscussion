using DiscussionServiceDataAccess.Configurations;
using DiscussionServiceDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscussionServiceDataAccess.DatabaseContext
{
    public class DiscussionDbContext : DbContext
    {
        public DbSet<Discussion> Discussions { get; set; } = null!;

        public DiscussionDbContext(DbContextOptions<DiscussionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscussionDbContext).Assembly);
            new DiscussionConfiguration().Configure(modelBuilder.Entity<Discussion>());
        }
    }
}
