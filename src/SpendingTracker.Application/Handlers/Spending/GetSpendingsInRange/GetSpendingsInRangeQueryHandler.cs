using SpendingTracker.Application.Handlers.Spending.GetSpendingsInRange.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInRange;

internal class GetSpendingsInRangeQueryHandler : QueryHandler<GetSpendingsInRangeQuery, GetSpendingsInRangeResponse>
{
    private readonly ISpendingRepository _spendingRepository;

    public GetSpendingsInRangeQueryHandler(ISpendingRepository spendingRepository)
    {
        _spendingRepository = spendingRepository;
    }

    public override async Task<GetSpendingsInRangeResponse> HandleAsync(
        GetSpendingsInRangeQuery inRangeQuery,
        CancellationToken cancellationToken)
    {
        var spendings = await _spendingRepository.GetUserSpendingsInRange(
            inRangeQuery.UserId,
            inRangeQuery.DateFrom,
            inRangeQuery.DateTo,
            cancellationToken);

        return new GetSpendingsInRangeResponse
        {
            Spending = spendings
        };
    }
}