using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Testcontainers.PostgreSql;
using UserServiceDataAccess.DatabaseHandlers.Seeder;
using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.Interfaces;

namespace MovieDiscussionTests.Integration.UserService
{
    public class TestWebClientFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly IConfiguration _configuration;
        private readonly PostgreSqlContainer _sqlServerContainer;

        public TestWebClientFactory()
        {
            _configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.test.json")
               .Build();

            int.TryParse(_configuration["Postgres:HostPort"], out var hostPort);
            int.TryParse(_configuration["Postgres:ExternalPort"], out var externalPort);

            _sqlServerContainer = new PostgreSqlBuilder()
               .WithImage(_configuration["Postgres:Image"])
               .WithUsername(_configuration["Postgres:User"])
               .WithPassword(_configuration["Postgres:Password"])
               .WithHostname(_configuration["Postgres:Host"])
               .WithDatabase(_configuration["Postgres:DbName"])
               .WithPortBinding(hostPort, externalPort)
               .Build();
        }

        public async Task InitializeAsync()
        {
            await _sqlServerContainer.StartAsync();

            InitializeSqlServerDatabase();
        }

        public new async Task DisposeAsync()
        {
            await _sqlServerContainer.StopAsync();

            await base.DisposeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureTestUsersDbContext);

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string> {
                        { "GrpcDeadline", "0" }
                    }
                );
            });
        }

        private void ConfigureTestUsersDbContext(IServiceCollection services)
        {
            SetupDbContext(services);

            var connectionString = _configuration["HangfireConnectionString"];
            SetupHangfireDbContext(services, connectionString);
            SetupHangfireServer(services, connectionString);

            UseSqlServerMigrations(services);
        }

        private static void SetupHangfireServer(IServiceCollection services, string? connectionString)
        {
            var hangfireServerDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(JobStorage));

            if (hangfireServerDescriptor is not null)
            {
                services.Remove(hangfireServerDescriptor);
            }

            services.AddHangfire(configuration => configuration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString))
            );
            services.AddHangfireServer();
        }

        private void SetupHangfireDbContext(IServiceCollection services, string? connectionString)
        {
            var hangfireDbDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<HangfireDbContext>));

            if (hangfireDbDescriptor is not null)
            {
                services.Remove(hangfireDbDescriptor);
            }

            services.AddDbContext<HangfireDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }

        private void SetupDbContext(IServiceCollection services)
        {
            var userDbDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<UserServiceDbContext>));

            if (userDbDescriptor is not null)
            {
                services.Remove(userDbDescriptor);
            }

            services.AddDbContext<UserServiceDbContext>(options =>
            {
                options.UseNpgsql(_sqlServerContainer.GetConnectionString());
            });
        }

        private void UseSqlServerMigrations(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
            var hangfireDbContext = scope.ServiceProvider.GetRequiredService<HangfireDbContext>();

            hangfireDbContext.Database.Migrate();

            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<UserSeeder>>();
            var userSeeder = new UserSeeder(dbContext, passwordHasher, logger);
            userSeeder.Seed();
        }

        private void InitializeSqlServerDatabase()
        {
            var originalConnString = _configuration["HangfireTestConnectionString"];
            var containerConnString = _sqlServerContainer.GetConnectionString();

            string extractedHost = ExtractHostname(containerConnString);
            string updatedConnString = ReplaceHostname(originalConnString, extractedHost);

            _configuration["HangfireConnectionString"] = updatedConnString;
        }

        private string ExtractHostname(string connStr)
        {
            Match match = Regex.Match(connStr, @"Host=([\w\.-]+);");

            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private string ReplaceHostname(string? connStr, string newHostname)
        {
            return Regex.Replace(connStr ?? string.Empty, @"Host=[\w\.-]+;", $"Host={newHostname};");
        }
    }
}