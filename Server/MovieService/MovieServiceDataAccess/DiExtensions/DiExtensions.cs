using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDataAccess.Repositories;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MovieServiceDataAccess.DatabaseContext;

namespace MovieServiceDataAccess.DiExtensions
{
    public static class DiExtensions
    {
        public static void AddMongo(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<MovieDbContext>(options =>
            {
                options.UseMongoDB(configuration["MovieDbConnection"]!, configuration["MovieDbName"]!);
            });

            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IWatchlistRepository, WatchlistRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            services.AddScoped<IUnitOfWork, MovieUnitOfWork>();
        }
    }
}
