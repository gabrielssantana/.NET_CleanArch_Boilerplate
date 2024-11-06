using AC.Core.Application.Interfaces;
using AC.Core.Domain.DTO.Queries;
using AC.Core.Domain.DTO.Queries.GetUsers;

using AutoMapper;

using MediatR;

namespace AC.Core.Application.Commands.Users.GetUsers
{
    public class GetUsersCommand : QueryParamsBase, IRequest<QueryResult<GetUsersCommandResultData>>, IMap
    {
        public string? Name { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetUsersCommand, GetUsersQueryParams>();
        }
    }
}