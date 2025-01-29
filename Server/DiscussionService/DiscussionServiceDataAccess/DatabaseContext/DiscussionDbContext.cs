using DiscussionServiceDomain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

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
            modelBuilder.Entity<Discussion>().ToCollection("discussions");
        }
    }
}
