using Microsoft.EntityFrameworkCore;
using UserGrpcServer.Extensions;
using UserGrpcServer.Services;
using UserServiceDataAccess.DatabaseHandlers.Repositories;
using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.DatabaseHandlers.UnitOfWork;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Interfaces.Repositories;

namespace UserGrpcServer
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);

            var services = builder.Services;
            string userDbConnectionString = Configuration["SqlConnectionString"] ?? throw new InvalidOperationException("No database connection string");

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddDbContext<UserServiceDbContext>(options => options.UseNpgsql(userDbConnectionString));
            //services.AddDbContext<HangfireDbContext>(options => options.UseNpgsql(hangfireConnectionString));

            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();

            builder.Services.AddGrpc(options => options.EnableDetailedErrors = true);

            builder.Host.AddLogging(Configuration);
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseCors();

            app.MapGrpcService<UserServiceImpl>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        }
    }
}
