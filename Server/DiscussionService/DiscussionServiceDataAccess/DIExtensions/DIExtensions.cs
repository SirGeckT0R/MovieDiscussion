using DiscussionServiceDataAccess.DatabaseContext;
using DiscussionServiceDataAccess.Interfaces.Repositories;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDataAccess.Repositories;
using DiscussionServiceDataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscussionServiceDataAccess.DIExtensions
{
    public static class DIExtensions
    {
        public static void AddMongo(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<DiscussionDbContext>(options =>
            {
                options.UseMongoDB(configuration["DiscussionDbConnection"]!, configuration["DiscussionDbName"]!);
            });

            services.AddScoped<IDiscussionRepository, DiscussionRepository>();

            services.AddScoped<IUnitOfWork, DiscussionUnitOfWork>();
        }
    }
}
