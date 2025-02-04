using Microsoft.EntityFrameworkCore;
using UserServiceDataAccess.DatabaseHandlers.Seeder;
using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.Interfaces;

namespace UserServiceWebAPI.Extensions
{
    public static class DatabaseExtensions
    {
        public static WebApplication SeedAndMigrateDatabases(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var hangfireDbContext = scope.ServiceProvider.GetRequiredService<HangfireDbContext>();
                hangfireDbContext.Database.EnsureCreated();
                hangfireDbContext.Database.Migrate();

                var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<UserSeeder>>();
                var userSeeder = new UserSeeder(dbContext, passwordHasher, logger);
                userSeeder.Seed();
            } 

            return app;
        }
    }
}
