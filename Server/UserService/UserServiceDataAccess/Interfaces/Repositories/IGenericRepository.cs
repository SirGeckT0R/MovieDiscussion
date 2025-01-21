using UserServiceDataAccess.DatabaseHandlers.Specifications;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : IdModel
    {
        Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<T?> GetByIdTrackingAsync(Guid id, CancellationToken cancellationToken);
        Task<T?> GetWithSpecificationAsync(Specification<T> specification, CancellationToken cancellationToken);
        Task<Guid> AddAsync(T model, CancellationToken cancellationToken);
        void Delete(T model, CancellationToken cancellationToken);
        void Update(T model, CancellationToken cancellationToken);
    }
}
