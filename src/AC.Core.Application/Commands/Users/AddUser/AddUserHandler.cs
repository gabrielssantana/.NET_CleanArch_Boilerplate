using AC.Core.Application.Interfaces.Data;
using AC.Core.Domain.Interfaces.Repositories;
using AC.Core.Domain.Models;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

namespace AC.Core.Application.Commands.Users.AddUser
{
    public class AddUserHandler(
        ILogger<AddUserHandler> logger,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : IRequestHandler<AddUserCommand, CommandResult<AddUserCommandResultData>>
    {
        private readonly ILogger<AddUserHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly CommandResult<AddUserCommandResultData> _result = new();
        public async Task<CommandResult<AddUserCommandResultData>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var novoUsuario = _mapper.Map<User>(request);
            _userRepository.Add(novoUsuario);
            var successAddingUser = await _unitOfWork.CommitAsync(cancellationToken);
            if (!successAddingUser)
            {
                return _result.AddError("Falha ao cadastrar usuário");
            }

            return _result.IsSuccess(_mapper.Map<AddUserCommandResultData>(novoUsuario), "Sucesso ao cadastrar usuário");
        }
    }
}