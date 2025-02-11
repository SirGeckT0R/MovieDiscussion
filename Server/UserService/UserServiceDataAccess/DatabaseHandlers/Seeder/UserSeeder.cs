using Microsoft.Extensions.Logging;
using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.DatabaseHandlers.Seeder
{
    public class UserSeeder(UserServiceDbContext context, IPasswordHasher passwordHasher, ILogger<UserSeeder> logger)
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly UserServiceDbContext _context = context;
        private readonly ILogger<UserSeeder> _logger = logger;

        public void Seed()
        {
            List<User> users =
            [
                    new User("example@example.com", "Jake", _passwordHasher.Generate("cool"), Role.User, true) { Id = Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29")},
                    new User("admin@example.com", "Mark", _passwordHasher.Generate("admin"), Role.Admin, true) { Id = Guid.Parse("453d01e1-533f-4609-9988-d243c89aca56")},
                    new User("great@example.com", "Victor", _passwordHasher.Generate("very"),  Role.User, true) { Id = Guid.Parse("2de57bf5-c74e-4d2f-b803-0ea8e6dc63ce")},
            ];

            if (!_context.Users.Any())
            {
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }

            _logger.LogInformation("Seeding completed");
        }
    }
}
