using System.Diagnostics.CodeAnalysis;
using MediatR;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Dispatcher.DataTransfer.Dispatcher
{
    /// <summary>
    /// Обёртка(враппер) для запроса, позволяющая понять
    /// медиатру как обработать и свалидировать запрос
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class QueryWrapper<TQuery, TResult> : IRequest<TResult>, IValidated
    {
        internal QueryWrapper(TQuery query)
        {
            Query = query;
        }

        public TQuery Query { get; }
        dynamic IValidated.InnerRequest => Query;
    }
}