using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Interfaces.Repositories;
using UserServiceDataAccess.ServiceDbContext;

namespace UserServiceDataAccess.UnitOfWork
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

        public void Save()
        {
            _context.SaveChanges();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                _context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
