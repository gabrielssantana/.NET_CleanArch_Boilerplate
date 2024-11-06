using AC.Core.Domain.Models;

namespace AC.Core.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string? email, CancellationToken cancellationToken = default);
    }
}