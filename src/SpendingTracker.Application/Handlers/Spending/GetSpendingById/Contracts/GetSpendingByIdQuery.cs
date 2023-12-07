using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingById.Contracts;

public class GetSpendingByIdQuery : IQuery<GetSpendingByIdResponse>
{
    public Guid Id { get; init; }
    public UserKey UserId { get; init; }
    public Guid? TargetCurrencyId { get; init; }
}