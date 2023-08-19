using SpendingTracker.Application.Spending.GetSpendings.Contracts;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Spending.GetSpendings;

internal class GetSpendingsQueryHandler : QueryHandler<GetSpendingsQuery, GetSpendingsQueryResponse>
{
    private readonly ISpendingRepository _spendingRepository;

    public GetSpendingsQueryHandler(ISpendingRepository spendingRepository)
    {
        _spendingRepository = spendingRepository;
    }

    public override async Task<GetSpendingsQueryResponse> HandleAsync(GetSpendingsQuery query, CancellationToken cancellationToken)
    {
        var spendings = await _spendingRepository.GetUserSpendings(
            new UserKey(query.UserId),
            query.DateFrom,
            query.DateTo,
            cancellationToken);

        return new GetSpendingsQueryResponse
        {
            Spending = spendings
        };
    }
}