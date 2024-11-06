
using AC.Core.Application.Interfaces.Data;
using AC.Core.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace AC.Core.Infrastructure.Data.Contexts
{
    public sealed class CoreDbContext(DbContextOptions<CoreDbContext> options) : DbContext(options), IUnitOfWork
    {
        public required DbSet<User> User { get; set; }
        public required DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned()).SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(cancellationToken) > 0;
        }
        public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.BeginTransactionAsync(cancellationToken);
        }

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.RollbackTransactionAsync(cancellationToken);
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.CommitTransactionAsync(cancellationToken);
        }
    }
}