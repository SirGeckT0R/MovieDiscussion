using MovieServiceDomain.Models;
using System.Linq.Expressions;

namespace MovieServiceDataAccess.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : IdModel
    {
        Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<T?> GetByIdTrackingAsync(Guid id, CancellationToken cancellationToken);
        //Task<T?> GetWithSpecificationAsync(Specification<T> specification, CancellationToken cancellationToken);
        Task<Guid> AddAsync(T model, CancellationToken cancellationToken);
        void Delete(T model, CancellationToken cancellationToken);
        void Update(T model, CancellationToken cancellationToken);
        //Task AddAsync(T entity);
        //Task<ICollection<T>> GetAllAsync();
        //Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        //Task<T> GetByIdAsync(Guid id);
        //Task<T> GetAsync(Expression<Func<T, bool>> filter);
        //Task RemoveAsync(Guid id);
        //Task UpdateAsync(T entity);
    }
}
