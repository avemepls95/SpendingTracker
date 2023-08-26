namespace SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;

public class GetSpendingsQueryResponse
{
    public Domain.Spending[] Spending { get; set; }
}