using Microsoft.EntityFrameworkCore;
using MovieServiceDataAccess.DatabaseContext;

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
            } 

            return app;
        }
    }
}
