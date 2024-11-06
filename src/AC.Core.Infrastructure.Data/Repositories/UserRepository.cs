using AC.Core.Domain.Interfaces.Repositories;
using AC.Core.Domain.Models;
using AC.Core.Infrastructure.Data.Contexts;

using Microsoft.EntityFrameworkCore;

namespace AC.Core.Infrastructure.Data.Repositories
{
    public class UserRepository(CoreDbContext coreDbContext) : RepositoryBase<User>(coreDbContext), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string? email, CancellationToken cancellationToken = default)
        {
            if (email is null)
            {
                return default;
            }

            return await _dbSet.FirstOrDefaultAsync(e => EF.Functions.ILike(e.Email, email), cancellationToken);
        }
    }
}