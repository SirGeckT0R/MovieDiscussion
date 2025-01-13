using Microsoft.EntityFrameworkCore;
using UserServiceDataAccess.Interfaces.Repositories;
using UserServiceDataAccess.Models;
using UserServiceDataAccess.ServiceDbContext;
using UserServiceDataAccess.Specifications;

namespace UserServiceDataAccess.Repositories
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

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _dbSet.Where(user => user.Id == id).ExecuteDeleteAsync(cancellationToken);
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

        public void Update(T model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _dbSet.Update(model);
        }

        protected IQueryable<T> ApplySpecification(Specification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbSet, specification);
        }
    }
}
