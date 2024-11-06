using AC.Core.Domain.Interfaces.Repositories;
using AC.Core.Infrastructure.Data.Contexts;

using Microsoft.EntityFrameworkCore;

namespace AC.Core.Infrastructure.Data.Repositories
{
    public abstract class RepositoryBase<TEntity>
    : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
    {
        protected readonly CoreDbContext _coreDbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext ?? throw new ArgumentNullException(nameof(coreDbContext));
            _dbSet = _coreDbContext.Set<TEntity>();
        }
        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void Dispose()
        {
            _coreDbContext.Dispose();
        }
    }
}