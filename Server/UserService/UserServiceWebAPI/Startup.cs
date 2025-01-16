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
using UserServiceDataAccess.DatabaseHandlers.Seeder;

namespace UserServiceWebAPI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            var services = builder.Services;
            string connection = Configuration["SqlConnectionString"] ?? throw new InvalidOperationException("No database connection string");
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));

            services.AddDbContext<UserServiceDbContext>(options => options.UseNpgsql(connection));

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

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(RegisterRequestMappingProfile)));
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
                using(var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
                    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                    var userSeeder = new UserSeeder(dbContext, passwordHasher);
                    userSeeder.Seed();
                }
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}

