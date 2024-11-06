using AC.Core.Application.Commands;

using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AC.Core.API.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/V{version:apiVersion}/[controller]")]
    [Produces("application/json"), Consumes("application/json")]
    public abstract class CoreControllerBase : ControllerBase
    {
        public IActionResult CoreResponse(CommandResult result)
        {
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}