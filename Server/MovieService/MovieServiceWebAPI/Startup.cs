using MovieServiceApplication.Extensions;
using MovieServiceApplication.UseCases.Genres.Commands.AddGenreCommand;
using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.DataSeeder;
using MovieServiceDataAccess.DiExtensions;
using MovieServiceWebAPI.ExceptionHandler;
using System.Reflection;

namespace MovieServiceWebAPI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            builder.Services.AddMongo(Configuration);

            builder.Services.AddMediatR();
            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AddGenreMappingProfile)));

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
                    var userSeeder = new DataSeeder(dbContext);
                    userSeeder.SeedAsync().GetAwaiter().GetResult();
                }
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.MapControllers();
            app.UseAuthorization();
        }
    }
}

