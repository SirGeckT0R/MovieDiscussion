using DiscussionServiceDataAccess.DatabaseContext;
using DiscussionServiceDataAccess.Seeder;

namespace DiscussionServiceWebAPI.Extensions
{
    public static class DatabaseExtensions
    {
        public static WebApplication SeedAndMigrateDatabases(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var hangfireDbContext = scope.ServiceProvider.GetRequiredService<HangfireDbContext>();
                hangfireDbContext.Database.EnsureCreated();

                var dbContext = scope.ServiceProvider.GetRequiredService<DiscussionDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataSeeder>>();
                var userSeeder = new DataSeeder(dbContext, logger);
                userSeeder.SeedAsync().GetAwaiter().GetResult();
            } 

            return app;
        }
    }
}
