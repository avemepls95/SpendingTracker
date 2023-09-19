namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Contracts;

public class GetSpendingsInDateRangeResponse
{
    public Domain.Spending[] Spending { get; set; }
}