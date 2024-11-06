using System.Net;

using AC.Core.Application.Commands;

using Newtonsoft.Json;

namespace AC.Core.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                var commandResult = new CommandResult();
                commandResult.AddError(JsonConvert.SerializeObject(error));
                var result = JsonConvert.SerializeObject(commandResult);
                _logger.LogError(error, error.Message);
                await response.WriteAsync(result);
            }
        }
    }
}