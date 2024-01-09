using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Account.GetAccountsInfo.Contracts;

public class GetAccountsInfoQuery : IQuery<GetAccountsInfoResponse>
{
    public UserKey UserId { get; set; }
    public Guid CurrencyId { get; set; }
}