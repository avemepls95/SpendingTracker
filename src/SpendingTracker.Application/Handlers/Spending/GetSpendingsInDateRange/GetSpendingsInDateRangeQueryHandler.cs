using SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange;

internal class GetSpendingsInDateRangeQueryHandler : QueryHandler<GetSpendingsInDateRangeQuery, GetSpendingsInDateRangeResponse>
{
    private readonly ISpendingRepository _spendingRepository;

    public GetSpendingsInDateRangeQueryHandler(ISpendingRepository spendingRepository)
    {
        _spendingRepository = spendingRepository;
    }

    public override async Task<GetSpendingsInDateRangeResponse> HandleAsync(
        GetSpendingsInDateRangeQuery inDateRangeQuery,
        CancellationToken cancellationToken)
    {
        var spendings = await _spendingRepository.GetUserSpendingsInRange(
            inDateRangeQuery.UserId,
            inDateRangeQuery.DateFrom,
            inDateRangeQuery.DateTo,
            cancellationToken);

        return new GetSpendingsInDateRangeResponse
        {
            Spending = spendings
        };
    }
}