namespace SpendingTracker.WebApp.Contracts.GetCategoriesAnalyticsByRange;

public class GetCategoriesAnalyticsByRangeRequest
{
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public Guid TargetCurrencyId { get; init; }
}