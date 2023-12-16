namespace SpendingTracker.Application.Handlers.Analytics.GetCategoriesAnalyticsByRange.Contracts;

public class GetCategoriesAnalyticsByRangeResponse
{
    public double TotalAmount { get; set; }

    public GetCategoriesAnalyticsByRangeResponseItem[] CategoryInfos { get; set; } =
        Array.Empty<GetCategoriesAnalyticsByRangeResponseItem>();
}