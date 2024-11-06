using AC.Core.Application.Interfaces;
using AC.Core.Domain.DTO.Queries.GetUsers;

using AutoMapper;

namespace AC.Core.Application.Commands.Users.GetUsers
{
    public class GetUsersCommandResultData : IMap
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? CompanyId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetUsersQueryResultData, GetUsersCommandResultData>();
        }
    }
}