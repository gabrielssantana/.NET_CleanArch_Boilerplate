using AC.Core.Domain.DTO.Queries;
using AC.Core.Domain.DTO.Queries.GetUsers;
using AC.Core.Domain.Interfaces.Queries.GetUsers;
using AC.Core.Infrastructure.Data.Contexts;

using Microsoft.EntityFrameworkCore;

namespace AC.Core.Infrastructure.Data.Queries
{
    public class GetUsersQuery(
        CoreDbContext coreDbContext
    ) : IGetUsersQuery
    {
        private readonly CoreDbContext _coreDbContext = coreDbContext ?? throw new ArgumentNullException(nameof(coreDbContext));
        private readonly QueryResult<GetUsersQueryResultData> _result = new();
        public Task<QueryResult<GetUsersQueryResultData>> ExecuteAsync(GetUsersQueryParams parameters, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var queryResultData = _coreDbContext.User
                                        .Where(u =>
                                            (!string.IsNullOrEmpty(parameters.Name) &&
                                            EF.Functions.ILike(u.Name, $"%{parameters.Name}%")) ||
                                            string.IsNullOrEmpty(parameters.Name)
                                        )
                                        .Select(u => new GetUsersQueryResultData
                                        {
                                            Name = u.Name,
                                            Document = u.Document,
                                            Phone = u.Phone,
                                            Status = u.Status,
                                            Email = u.Email,
                                            CompanyId = u.CompanyId,
                                        });
                if (!parameters.Paginate)
                {
                    _result.Data = queryResultData;
                    return _result;
                }

                var count = queryResultData.Count();
                queryResultData = queryResultData
                                    .Skip(parameters.IgnoredItems)
                                    .Take(parameters.ItemsPerPage);
                _result.TotalItems = count;
                _result.CurrentPage = parameters.CurrentPage;
                _result.ItemsPerPage = parameters.ItemsPerPage;
                _result.Data = queryResultData;
                return _result;
            }, cancellationToken);
        }
    }
}