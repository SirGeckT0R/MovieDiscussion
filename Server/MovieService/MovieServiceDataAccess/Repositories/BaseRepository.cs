using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDataAccess.LinqExtensions;
using MovieServiceDataAccess.Specifications;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Repositories
{
    public class BaseRepository<T>(MovieDbContext dbContext) : IBaseRepository<T> where T : IdModel
    {
        protected readonly MovieDbContext _dbContext = dbContext;
        protected DbSet<T> _dbSet = dbContext.Set<T>();

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

        public async Task<ICollection<T>> GetFromListOfIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _dbSet.AsNoTracking().Where(user => ids.Contains(user.Id)).ToListAsync(cancellationToken);
        }

        public async Task<ICollection<T>> GetWithSpecificationAsync(Specification<T> specification, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var collection = await ApplySpecification(specification);

            return collection;
        }

        public async Task<(ICollection<T>, int)> GetPaginatedWithSpecificationAsync(
                                                                                    Specification<T> specification,
                                                                                    int pageIndex,
                                                                                    int pageSize,
                                                                                    CancellationToken cancellationToken
                                                                                   )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var filteredCollection = SpecificationEvaluator<T>.GetQuery(_dbSet, specification);
            var paginatedCollection = await filteredCollection.TrySkip((pageIndex - 1) * pageSize)
                                                              .TryTake(pageSize)
                                                              .ToListAsync(cancellationToken);

            var amountOfFilteredItems = await filteredCollection.CountAsync();
            int totalPages = (int)Math.Ceiling(amountOfFilteredItems / (float)pageSize);

            return (paginatedCollection, totalPages);
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
