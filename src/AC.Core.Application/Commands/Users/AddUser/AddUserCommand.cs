using AC.Core.Application.Interfaces;
using AC.Core.Domain.Models;

using AutoMapper;

using MediatR;

namespace AC.Core.Application.Commands.Users.AddUser
{
    public class AddUserCommand : IRequest<CommandResult<AddUserCommandResultData>>, IMap
    {
        public string? Name { get; set; }
        public string? Document { get; set; }
        public string? Phone { get; set; }
        public bool? Status { get; set; }
        public string? Email { get; set; }
        public int? CompanyId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddUserCommand, User>();
        }
    }
}