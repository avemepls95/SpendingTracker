using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.UserCurrency.ChangeUserCurrency.Contracts;

public class ChangeUserCurrencyCommand : ICommand
{
    public UserKey UserId { get; init; }
    public string CurrenctCode { get; init; }
}