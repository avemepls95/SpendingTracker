using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.UserCurrency.ChangeUserCurrency.Contracts;

public class ChangeUserCurrencyCommand : ISpendingTrackerCommand
{
    public UserKey UserId { get; init; }
    public string CurrencyCode { get; init; }
    public ActionSource ActionSource { get; init; }
}