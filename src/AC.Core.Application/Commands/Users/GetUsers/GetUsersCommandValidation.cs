using AC.Core.Domain.Interfaces.Repositories;

using FluentValidation;

namespace AC.Core.Application.Commands.Users.GetUsers
{
    public class GetUsersCommandValidation : AbstractValidator<GetUsersCommand>
    {
        public GetUsersCommandValidation() { }
    }
}