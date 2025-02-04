﻿using DiscussionServiceApplication.Extensions;
using DiscussionServiceApplication.MappingProfiles;
using DiscussionServiceDataAccess.DatabaseContext;
using DiscussionServiceDataAccess.DIExtensions;
using DiscussionServiceWebAPI.Extensions;
using DiscussionServiceWebAPI.Hubs;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using MovieServiceWebAPI.ExceptionHandler;
using System.Reflection;
using System.Security.Claims;
using DiscussionServiceWebAPI.Hangfire;

namespace DiscussionServiceWebAPI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);

            builder.Services.AddStackExchangeRedisCache(options =>
                options.Configuration = builder.Configuration["Redis"]
            );

            var hangfireConnectionString = Configuration["HangfireConnectionString"]!;
            builder.Services.AddDbContext<HangfireDbContext>(options => options.UseMongoDB(hangfireConnectionString, "hangfire"));

            builder.Services.AddSignalR();

            builder.Services.AddMongo(Configuration);

            builder.Services.AddMediatR();
            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(DiscussionDtoMappingProfile)));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var migrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new DropMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            };
            builder.Services.AddHangfire(configuration => configuration
                                                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                                    .UseSimpleAssemblyNameTypeSerializer()
                                                    .UseRecommendedSerializerSettings()
                                                    .UseMongoStorage(hangfireConnectionString, "hangfire",
                                                        new MongoStorageOptions
                                                        {
                                                            MigrationOptions = migrationOptions,
                                                            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                                                        })
                                         );

            builder.Services.AddHangfireServer();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Host.AddLogging(Configuration);
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseExceptionHandler();

            app.UseAuthorization();


            var options = new DashboardOptions()
            {
                Authorization = [new HangfireAuthorizationFilter()]
            };
            app.UseHangfireDashboard("/hangfire", options);

            app.MapControllers();
            app.MapHangfireDashboard();
            app.MapHub<DiscussionHub>("discussion-hub");
        }
    }
}
