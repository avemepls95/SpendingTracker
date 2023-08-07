using MediatR;

namespace SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces
{
    public interface IQuery<out T> : IRequest<T>
    {
    }
}