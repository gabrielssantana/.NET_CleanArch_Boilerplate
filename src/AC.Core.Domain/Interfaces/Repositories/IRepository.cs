namespace AC.Core.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity>
    : IDisposable
    where TEntity : class, IAggregateRoot
    {
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}