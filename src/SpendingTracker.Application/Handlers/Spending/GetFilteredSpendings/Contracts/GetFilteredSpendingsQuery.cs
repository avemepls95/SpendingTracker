using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.GetFilteredSpendings.Contracts;

public class GetFilteredSpendingsQuery : IQuery<GetFilteredSpendingsResponseItem[]>
{
    public UserKey UserId { get; init; }
    public DateOnly DateFrom { get; init; }
    public DateOnly DateTo { get; init; }
    public Guid? TargetCurrencyId { get; init; }
    public Guid? CategoryId { get; init; }
}