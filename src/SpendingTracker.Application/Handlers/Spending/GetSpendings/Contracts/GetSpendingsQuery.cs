using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;

public class GetSpendingsQuery : IQuery<GetSpendingsResponseItem[]>
{
    public UserKey UserId { get; set; }
    public int Offset { get; set; } = 0;
    public int Count { get; set; } = 10;
}