using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;
using UserServiceDataAccess.ServiceDbContext;

namespace UserServiceDataAccess.Seeder
{
    public class UserSeeder(UserServiceDbContext context, IPasswordHasher passwordHasher)
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly UserServiceDbContext _context = context;

        public void Seed()
        {
            List<User> users = new List<User> {
                    new User("example@example.com", "Jake", _passwordHasher.Generate("cool"), E_Role.User),
                    new User("admin@example.com", "Mark", _passwordHasher.Generate("admin"), E_Role.Admin),
                    new User("great@example.com", "Victor", _passwordHasher.Generate("very"),  E_Role.User),
                };

            if (!_context.Users.Any())
            {
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }
    }
}
