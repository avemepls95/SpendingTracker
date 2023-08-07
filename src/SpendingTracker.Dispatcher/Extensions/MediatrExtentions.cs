using System.Diagnostics.CodeAnalysis;
using MediatR;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Dispatcher.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class MediatrExtensions
    {
        /// <summary>
        /// Отправить команду для изменения данных, которая затем будет обработана handler'ом
        /// </summary>
        /// <param name="mediator"><see cref="IMediator"/></param>
        /// <param name="command">Generic mediatr command </param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        public static async Task SendCommandAsync<TCommand>(
            this IMediator mediator,
            TCommand command,
            CancellationToken cancellationToken) where TCommand : ICommand
        {
            await mediator.Send(new CommandWrapper<TCommand>(command), cancellationToken);
        }

        // <summary>
        /// Отправить команду для изменения данных, которая затем будет обработана handler'ом
        /// и получить результат команды
        /// </summary>
        /// <param name="mediator"><see cref="IMediator"/></param>
        /// <param name="command">Generic mediatr command </param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        public static async Task<TResult> SendCommandAsync<TCommand, TResult>(
            this IMediator mediator,
            TCommand command,
            CancellationToken cancellationToken) where TCommand : ICommand<TResult>
        {
            var result = await mediator.Send(new CommandWrapper<TCommand, TResult>(command), cancellationToken);
            return result;
        }

        /// <summary>
        /// Отправить запрос на получение данных, который
        /// затем обработается соответствующим handler"ом
        /// и получить результат
        /// </summary>
        public static async Task<TResult> SendQueryAsync<TQuery, TResult>(
            this IMediator mediator,
            TQuery query,
            CancellationToken cancellationToken) where TQuery : IQuery<TResult>
        {
            var result = await mediator.Send(new QueryWrapper<TQuery,TResult>(query), cancellationToken);
            return result;
        }
    }
}