using UserServiceDataAccess.Interfaces.Repositories;

namespace UserServiceDataAccess.Interfaces
{
    public interface IUserUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ITokenRepository TokenRepository { get; }
        void Save();
    }
}
