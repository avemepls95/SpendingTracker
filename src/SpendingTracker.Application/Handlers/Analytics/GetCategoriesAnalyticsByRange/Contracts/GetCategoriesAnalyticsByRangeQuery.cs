using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Analytics.GetCategoriesAnalyticsByRange.Contracts;

public class GetCategoriesAnalyticsByRangeQuery : IQuery<GetCategoriesAnalyticsByRangeResponse>
{
    public DateOnly DateFrom { get; init; }
    public DateOnly DateTo { get; init; }
    public UserKey UserId { get; init; }   
    public Guid TargetCurrencyId { get; init; }
}