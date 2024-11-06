namespace AC.Core.Domain.Interfaces.Queries
{
    public interface IQuery<TParams, TResult>
    where TParams : class
    where TResult : class
    {
        Task<TResult> ExecuteAsync(TParams parameters, CancellationToken cancellationToken = default);
    }
}