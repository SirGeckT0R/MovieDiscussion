using UserServiceDataAccess.Handlers;
using UserServiceDataAccess.Interfaces.Repositories;
using UserServiceDataAccess.Interfaces;
using UserServiceWebAPI.ExceptionHandler;
using Microsoft.EntityFrameworkCore;
using UserServiceApplication.Interfaces.Services;
using UserServiceApplication.Services;
using UserServiceWebAPI.Extensions;
using UserServiceApplication.MappingProfiles;
using System.Reflection;
using UserServiceApplication.Validators;
using UserServiceDataAccess.Models;
using FluentValidation;
using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.DatabaseHandlers.Repositories;
using UserServiceDataAccess.DatabaseHandlers.UnitOfWork;
using UserServiceDataAccess.PasswordHasher;
using Hangfire;
using Hangfire.PostgreSql;
using UserServiceWebAPI.Hangfire;

namespace UserServiceWebAPI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            var services = builder.Services;
            string userDbConnectionString = Configuration["SqlConnectionString"] ?? throw new InvalidOperationException("No database connection string");
            var hangfireConnectionString = Configuration["HangfireConnectionString"];

            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));

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
            services.AddDbContext<HangfireDbContext>(options => options.UseNpgsql(hangfireConnectionString));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();

            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddScoped<IValidator<Token>, TokenValidator>();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddApiAuthentication(Configuration);

            services.AddHangfire(configuration => configuration
                                                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                                    .UseSimpleAssemblyNameTypeSerializer()
                                                    .UseRecommendedSerializerSettings()
                                                    .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(hangfireConnectionString!))
                                );

            services.AddHangfireServer();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(RegisterRequestMappingProfile)));

            builder.Host.AddLogging(Configuration);
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
            });

            app.UseDefaultFiles();

            if (env.IsDevelopment())
            {
                app.SeedAndMigrateDatabases();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.SeedAndMigrateDatabases();
            var options = new DashboardOptions()
            {
                Authorization = [new HangfireAuthorizationFilter()]
            };
            app.UseHangfireDashboard("/hangfire", options);

            app.UseExceptionHandler();
            app.MapControllers();
            app.MapHangfireDashboard();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}

