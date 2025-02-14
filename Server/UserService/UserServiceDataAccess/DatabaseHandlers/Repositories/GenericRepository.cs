using Microsoft.EntityFrameworkCore;
using UserServiceDataAccess.DatabaseHandlers.ServiceDbContext;
using UserServiceDataAccess.DatabaseHandlers.Specifications;
using UserServiceDataAccess.Interfaces.Repositories;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.DatabaseHandlers.Repositories
{
    public class GenericRepository<T>(UserServiceDbContext context) : IGenericRepository<T> where T : IdModel
    {
        protected readonly UserServiceDbContext _dbContext = context;
        protected DbSet<T> _dbSet = context.Set<T>();

        public async Task<Guid> AddAsync(T model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _dbSet.AddAsync(model, cancellationToken);

            return result.Entity.Id;
        }

        public void Delete(T model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _dbSet.Remove(model);
        }

        public async Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public async Task<T?> GetByIdTrackingAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _dbSet.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public async Task<ICollection<T>> GetWithSpecificationAsync(Specification<T> specification, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var collection = await ApplySpecification(specification);

            return collection;
        }

        public void Update(T model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _dbSet.Update(model);
        }

        protected async Task<ICollection<T>> ApplySpecification(Specification<T> specification)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbSet, specification).ToListAsync();
        }
    }
}
