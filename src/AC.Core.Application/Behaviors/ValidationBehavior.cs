using AC.Core.Application.Commands;

using FluentValidation;

using MediatR;

namespace AC.Core.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull, CommandResult
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                                                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                                            );
            var errorMessages = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .Select(r => r.ErrorMessage)
                .ToList();
            if (errorMessages.Any())
            {
                var result = Activator.CreateInstance(typeof(TResponse), errorMessages) as TResponse ?? throw new Exception("All commands must return CommandResult");
                return result;
            }

            return await next();
        }
    }
}