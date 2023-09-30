using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendings.Converters;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendings;

internal class GetSpendingsQueryHandler : QueryHandler<GetSpendingsQuery, GetSpendingsResponseItem[]>
{
    private readonly ISpendingRepository _spendingRepository;

    public GetSpendingsQueryHandler(ISpendingRepository spendingRepository)
    {
        _spendingRepository = spendingRepository;
    }

    public override async Task<GetSpendingsResponseItem[]> HandleAsync(
        GetSpendingsQuery inDateRangeQuery,
        CancellationToken cancellationToken)
    {
        var spendings = await _spendingRepository.GetUserSpendings(
            inDateRangeQuery.UserId,
            inDateRangeQuery.Offset,
            inDateRangeQuery.Count,
            cancellationToken);

        var result = spendings
            .Select(SpendingConverter.ConvertToDto)
            .ToArray();

        return result;
    }
}