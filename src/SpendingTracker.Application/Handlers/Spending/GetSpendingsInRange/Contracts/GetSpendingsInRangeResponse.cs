namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInRange.Contracts;

public class GetSpendingsInRangeResponse
{
    public Domain.Spending[] Spending { get; set; }
}