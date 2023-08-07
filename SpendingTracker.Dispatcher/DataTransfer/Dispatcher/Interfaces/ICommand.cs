using MediatR;

namespace SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out T> : IRequest<T>
    {
    }
}