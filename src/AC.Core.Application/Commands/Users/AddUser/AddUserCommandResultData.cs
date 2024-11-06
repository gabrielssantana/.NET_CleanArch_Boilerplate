using AC.Core.Application.Interfaces;
using AC.Core.Domain.Models;

using AutoMapper;

namespace AC.Core.Application.Commands.Users.AddUser
{
    public class AddUserCommandResultData : IMap
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, AddUserCommandResultData>();
        }
    }
}