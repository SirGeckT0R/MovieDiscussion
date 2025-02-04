using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.DataSeeder;

namespace MovieServiceWebAPI.Extensions
{
    public static class DatabaseExtensions
    {
        public static WebApplication SeedAndMigrateDatabases(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var hangfireDbContext = scope.ServiceProvider.GetRequiredService<HangfireDbContext>();
                hangfireDbContext.Database.EnsureCreated();

                var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataSeeder>>();
                var userSeeder = new DataSeeder(dbContext, logger);
                userSeeder.SeedAsync().GetAwaiter().GetResult();
            } 

            return app;
        }
    }
}
