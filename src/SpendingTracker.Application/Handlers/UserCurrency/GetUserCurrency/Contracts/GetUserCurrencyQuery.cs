using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.UserCurrency.GetUserCurrency.Contracts;

public class GetUserCurrencyQuery : IQuery<Currency>
{
    public UserKey UserKey { get; set; }
}