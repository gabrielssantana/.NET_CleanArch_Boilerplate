using AC.Core.Domain.DTO.Queries.GetUsers;
using AC.Core.Domain.Interfaces.Queries.GetUsers;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

namespace AC.Core.Application.Commands.Users.GetUsers
{
    public class GetUsersHandler(
        ILogger<GetUsersHandler> logger,
        IGetUsersQuery getUsersQuery,
        IMapper mapper
    ) : IRequestHandler<GetUsersCommand, QueryResult<GetUsersCommandResultData>>
    {
        private readonly ILogger<GetUsersHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IGetUsersQuery _getUsersQuery = getUsersQuery ?? throw new ArgumentNullException(nameof(getUsersQuery));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly QueryResult<GetUsersCommandResultData> _result = new();
        public async Task<QueryResult<GetUsersCommandResultData>> Handle(GetUsersCommand request, CancellationToken cancellationToken)
        {
            var queryResult = await _getUsersQuery.ExecuteAsync(_mapper.Map<GetUsersQueryParams>(request), cancellationToken);
            _mapper.Map(queryResult, _result);
            return _result.IsSuccess();
        }
    }
}