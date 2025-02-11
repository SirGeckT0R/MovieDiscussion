using MovieGrpcServer.Extensions;
using MovieGrpcServer.Services;
using MovieServiceDataAccess.DiExtensions;

namespace MovieGrpcServer
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            builder.Services.AddMongo(Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddGrpc(options => options.EnableDetailedErrors = true);

            builder.Host.AddLogging(Configuration);
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseCors();

            app.MapGrpcService<MovieServiceImpl>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        }
    }
}
