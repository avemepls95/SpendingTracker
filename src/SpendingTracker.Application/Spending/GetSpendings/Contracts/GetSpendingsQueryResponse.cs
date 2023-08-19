namespace SpendingTracker.Application.Spending.GetSpendings.Contracts;

public class GetSpendingsQueryResponse
{
    public Domain.Spending[] Spending { get; set; }
}