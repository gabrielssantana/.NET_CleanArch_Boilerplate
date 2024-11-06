using AC.Core.Domain.Interfaces.Repositories;
using AC.Core.Domain.Models;
using AC.Core.Infrastructure.Data.Contexts;

namespace AC.Core.Infrastructure.Data.Repositories
{
    public class CompanyRepository(CoreDbContext coreDbContext) : RepositoryBase<Company>(coreDbContext), ICompanyRepository { }
}