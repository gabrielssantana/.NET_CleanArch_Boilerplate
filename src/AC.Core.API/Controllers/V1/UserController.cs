using AC.Core.Application.Commands;
using AC.Core.Application.Commands.Users.AddUser;
using AC.Core.Application.Commands.Users.GetUsers;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace AC.Core.API.Controllers.V1
{
    [AllowAnonymous]
    [SwaggerTag("Reúne endpoints para gerenciamento de usuários.")]
    public class UserController(IMediator mediator) : CoreControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Realiza o cadastro de um usuário.
        /// </summary>
        /// <param name="command"></param>
        /// <response code="200">Sucesso, verifique messages.</response>
        /// <response code="400">Erro tratado, verifique messages.</response>
        /// <response code="500">Erro inesperado, verifique messages.</response>
        [HttpPost("AddUser")]
        [SwaggerOperation(Description = "Endpoint para realizar a criação de um usuário.")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(CommandResult<int>))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(CommandResult))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(CommandResult))]
        public async Task<IActionResult> AddUser(AddUserCommand command)
        {
            return CoreResponse(await _mediator.Send(command));
        }
        /// <summary>
        /// Realiza a busca usuários.
        /// </summary>
        /// <param name="command"></param>
        /// <response code="200">Sucesso, verifique messages.</response>
        /// <response code="400">Erro tratado, verifique messages.</response>
        /// <response code="500">Erro inesperado, verifique messages.</response>
        [HttpGet("GetUsers")]
        [SwaggerOperation(Description = "Endpoint para realizar a busca de usuários.")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(QueryResult<GetUsersCommandResultData>))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(CommandResult))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(CommandResult))]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersCommand command)
        {
            return CoreResponse(await _mediator.Send(command));
        }
    }
}