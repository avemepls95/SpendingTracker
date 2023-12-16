using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Contracts;

public class GetSpendingsInDateRangeQuery : IQuery<GetSpendingsInDateRangeResponseItem[]>
{
    public UserKey UserId { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public Guid? TargetCurrencyId { get; init; }
}