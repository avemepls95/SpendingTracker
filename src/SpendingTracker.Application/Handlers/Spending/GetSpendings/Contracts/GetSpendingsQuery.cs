using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;

public class GetSpendingsQuery : IQuery<GetSpendingsResponseItem[]>
{
    public UserKey UserId { get; init; }
    public int Offset { get; init; } = 0;
    public int Count { get; init; } = 10;
}