using AC.Core.Application.Commands;
using AC.Core.Application.Interfaces.Data;

using MediatR;

namespace AC.Core.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse>(
        IUnitOfWork unitOfWork
    ) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull, CommandResult
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var result = await next();
            if (!result.Success)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return result;
            }
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
    }
}