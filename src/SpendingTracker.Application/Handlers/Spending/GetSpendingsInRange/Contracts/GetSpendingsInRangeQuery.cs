using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInRange.Contracts;

public class GetSpendingsInRangeQuery : IQuery<GetSpendingsInRangeResponse>
{
    public UserKey UserId { get; set; }
    public DateTimeOffset DateFrom { get; set; }
    public DateTimeOffset DateTo { get; set; }
}