using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Contracts;

public class GetSpendingsInDateRangeQuery : IQuery<GetSpendingsInDateRangeResponse>
{
    public UserKey UserId { get; set; }
    public DateTimeOffset DateFrom { get; set; }
    public DateTimeOffset DateTo { get; set; }
}