using DiscussionServiceDataAccess.Specifications;
using DiscussionServiceDomain.Models;

namespace DiscussionServiceDataAccess.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : IdModel
    {
        Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<T?> GetByIdTrackingAsync(Guid id, CancellationToken cancellationToken);
        Task<ICollection<T>> GetWithSpecificationAsync(Specification<T> specification, CancellationToken cancellationToken);
        Task<Guid> AddAsync(T model, CancellationToken cancellationToken);
        void Delete(T model, CancellationToken cancellationToken);
        void Update(T model, CancellationToken cancellationToken);
    }
}
