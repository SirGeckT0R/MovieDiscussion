using DiscussionServiceApplication.Extensions;
using DiscussionServiceApplication.MappingProfiles;
using DiscussionServiceDataAccess.DIExtensions;
using DiscussionServiceWebAPI.Hubs;
using MovieServiceWebAPI.ExceptionHandler;
using System.Reflection;
using System.Security.Claims;

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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseExceptionHandler();

            app.UseAuthorization();

            app.MapHub<DiscussionHub>("discussion-hub");
        }
    }
}
