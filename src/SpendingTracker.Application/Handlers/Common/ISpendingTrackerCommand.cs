using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Common;

public interface ISpendingTrackerCommand : ICommand
{
    public ActionSource ActionSource { get; init; }
}