using System.Diagnostics.CodeAnalysis;
using MediatR;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Dispatcher.DataTransfer.Dispatcher
{
    /// <summary>
    /// Обёртка команды позволяющая понять медиатру
    /// куда отправить команду, и как её завалидировать
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CommandWrapper<TCommand> : IRequest, IValidated
    {
        internal CommandWrapper(TCommand command)
        {
            Command = command;
        }

        public TCommand Command { get; }
        dynamic IValidated.InnerRequest => Command;
    }

    /// <summary>
    /// Обёртка команды позволяющая понять медиатру
    /// куда отправить команду, и как её завалидировать
    /// с возвращаемым результатом TResult
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CommandWrapper<TCommand, TResult> : IRequest<TResult>, IValidated
    {
        internal CommandWrapper(TCommand command)
        {
            Command = command;
        }

        public TCommand Command { get; }
        dynamic IValidated.InnerRequest => Command;
    }
}