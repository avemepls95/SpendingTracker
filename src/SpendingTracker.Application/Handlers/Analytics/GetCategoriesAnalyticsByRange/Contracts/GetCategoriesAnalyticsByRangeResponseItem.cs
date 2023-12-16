namespace SpendingTracker.Application.Handlers.Analytics.GetCategoriesAnalyticsByRange.Contracts;

public class GetCategoriesAnalyticsByRangeResponseItem
{
    public Guid CategoryId { get; init; }
    public string CategoryTitle { get; init; }

    public List<GetCategoriesAnalyticsByRangeResponseItem> Childs { get; } = new();

    public List<Guid> ParentIds { get; } = new();

    public double Amount { get; set; }
}