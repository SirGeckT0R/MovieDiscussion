using Microsoft.EntityFrameworkCore;
using UserServiceDataAccess.Configurations;
using UserServiceDataAccess.Interceptors;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.ServiceDbContext
{
    public class UserServiceDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Token> Tokens { get; set; } = null!;
        public UserServiceDbContext() { }
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new TokenConfiguration().Configure(modelBuilder.Entity<Token>());
        }
    }
}
