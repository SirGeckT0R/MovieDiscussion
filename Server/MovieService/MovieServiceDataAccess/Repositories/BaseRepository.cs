using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDomain.Models;
using System.Linq.Expressions;

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

        //public async Task<T?> GetWithSpecificationAsync(Specification<T> specification, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    return (await ApplySpecification(specification)).FirstOrDefault();
        //}

        public void Update(T model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _dbSet.Update(model);
        }

        //protected async Task<ICollection<T>> ApplySpecification(Specification<T> specification)
        //{
        //    return await SpecificationEvaluator<T>.GetQuery(_dbSet, specification).ToListAsync();
        //}



        //public BaseRepository(IMongoDatabase database, string collectionName)
        //{
        //    collection = database.GetCollection<T>(collectionName);
        //}

        //public async Task<ICollection<T>> GetAllAsync()
        //{
        //    return await collection.Find(filterBuilder.Empty).ToListAsync();
        //}

        //public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        //{
        //    return await collection.Find(filter).ToListAsync();
        //}

        //public async Task<T> GetByIdAsync(Guid id)
        //{
        //    FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, id);
        //    return await collection.Find(filter).FirstOrDefaultAsync();
        //}

        //public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        //{
        //    return await collection.Find(filter).FirstOrDefaultAsync();
        //}

        //public async Task AddAsync(T entity)
        //{
        //    await collection.InsertOneAsync(entity);
        //}

        //public async Task UpdateAsync(T entity)
        //{
        //    FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, entity.Id);
        //    await collection.ReplaceOneAsync(filter, entity);
        //}

        //public async Task RemoveAsync(Guid id)
        //{
        //    FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, id);
        //    await collection.DeleteOneAsync(filter);
        //}
    }
}
