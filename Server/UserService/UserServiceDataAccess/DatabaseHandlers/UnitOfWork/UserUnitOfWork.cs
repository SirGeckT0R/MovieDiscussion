using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Interfaces.Repositories;

namespace UserServiceDataAccess.DatabaseHandlers.UnitOfWork
{
    public class UserUnitOfWork(UserServiceDbContext context, IUserRepository userRepository, ITokenRepository tokenRepository) : IUserUnitOfWork
    {
        private readonly UserServiceDbContext _context = context;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenRepository _tokenRepository = tokenRepository;

        private bool disposed = false;

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository;
            }
        }

        public ITokenRepository TokenRepository
        {
            get
            {
                return _tokenRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
