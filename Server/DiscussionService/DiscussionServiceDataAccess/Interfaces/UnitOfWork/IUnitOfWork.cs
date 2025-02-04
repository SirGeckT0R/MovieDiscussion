using DiscussionServiceDataAccess.Interfaces.Repositories;

namespace DiscussionServiceDataAccess.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDiscussionRepository Discussions { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
