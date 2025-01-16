using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.DatabaseHandlers.Seeder
{
    public class UserSeeder(UserServiceDbContext context, IPasswordHasher passwordHasher)
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly UserServiceDbContext _context = context;

        public void Seed()
        {
            List<User> users = new()
            {
                    new User("example@example.com", "Jake", _passwordHasher.Generate("cool"), ERole.User, true),
                    new User("admin@example.com", "Mark", _passwordHasher.Generate("admin"), ERole.Admin, true),
                    new User("great@example.com", "Victor", _passwordHasher.Generate("very"),  ERole.User, true),
                };

            if (!_context.Users.Any())
            {
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }
    }
}
