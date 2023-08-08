using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.UserCurrency.GetUserCurrency;

public class GetUserCurrencyQuery : IQuery<Currency>
{
    public UserKey UserKey { get; set; }
}