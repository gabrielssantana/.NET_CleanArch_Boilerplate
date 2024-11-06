using AC.Core.Domain.DTO.Queries;
using AC.Core.Domain.DTO.Queries.GetUsers;

namespace AC.Core.Domain.Interfaces.Queries.GetUsers
{
    public interface IGetUsersQuery : IQuery<GetUsersQueryParams, QueryResult<GetUsersQueryResultData>> { }
}