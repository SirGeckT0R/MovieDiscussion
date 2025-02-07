using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Microsoft.EntityFrameworkCore;
using MovieServiceApplication.Extensions;
using MovieServiceApplication.UseCases.Genres.Commands.AddGenreCommand;
using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.DiExtensions;
using MovieServiceWebAPI.ExceptionHandler;
using MovieServiceWebAPI.Hangfire;
using System.Reflection;
using MovieServiceWebAPI.Extensions;

namespace MovieServiceWebAPI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            builder.Services.AddMongo(Configuration);

            var hangfireConnectionString = Configuration["HangfireConnectionString"]!;
            builder.Services.AddDbContext<HangfireDbContext>(options => options.UseMongoDB(hangfireConnectionString, "hangfire"));

            builder.Services.AddMediatR();
            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AddGenreMappingProfile)));

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var migrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new DropMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            };
            builder.Services.AddHangfire(configuration => configuration
                                                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                                    .UseSimpleAssemblyNameTypeSerializer()
                                                    .UseRecommendedSerializerSettings()
                                                    .UseMongoStorage(hangfireConnectionString, 
                                                                        "hangfire", 
                                                                        new MongoStorageOptions 
                                                                        { 
                                                                            MigrationOptions = migrationOptions, 
                                                                            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection 
                                                                        }
                                                                    )
                                         );

            builder.Services.AddHangfireServer();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Host.AddLogging(Configuration);
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.SeedAndMigrateDatabases();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var options = new DashboardOptions()
            {
                Authorization = [new HangfireAuthorizationFilter()]
            };
            app.UseHangfireDashboard("/hangfire", options);

            app.UseExceptionHandler();
            app.MapControllers();

            app.MapHangfireDashboard();

            app.UseAuthorization();
        }
    }
}

