using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace SpendingTracker.Dispatcher.DataTransfer.Dispatcher
{
    /// <summary>
    /// Абстрактный класс для реализации обработчика запросов на получение данных
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class QueryHandler<TQuery, TResult> :
        IRequestHandler<QueryWrapper<TQuery, TResult>, TResult>
    {
        public async Task<TResult> Handle(QueryWrapper<TQuery, TResult> request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request.Query, cancellationToken);
        }

        public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
    }
}