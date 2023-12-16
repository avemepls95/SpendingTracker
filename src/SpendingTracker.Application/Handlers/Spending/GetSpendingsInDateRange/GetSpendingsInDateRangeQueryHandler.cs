using SpendingTracker.Application.Handlers.Spending.GetSpendings.Converters;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Contracts;
using SpendingTracker.Application.Handlers.Spending.Services.Abstractions;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange;

internal class GetSpendingsInDateRangeQueryHandler
    : QueryHandler<GetSpendingsInDateRangeQuery, GetSpendingsInDateRangeResponseItem[]>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly ISpendingsConvertingService _spendingsConvertingService;

    public GetSpendingsInDateRangeQueryHandler(
        ISpendingRepository spendingRepository,
        ISpendingsConvertingService spendingsConvertingService)
    {
        _spendingRepository = spendingRepository;
        _spendingsConvertingService = spendingsConvertingService;
    }

    public override async Task<GetSpendingsInDateRangeResponseItem[]> HandleAsync(
        GetSpendingsInDateRangeQuery query,
        CancellationToken cancellationToken)
    {
        var spendings = await _spendingRepository.GetUserSpendingsInRange(
            query.UserId,
            query.DateFrom,
            query.DateTo,
            cancellationToken);

        GetSpendingsInDateRangeResponseItem[] result;
        if (query.TargetCurrencyId.HasValue)
        {
            var spendingConvertedAmountDict = await _spendingsConvertingService.GetSpendingsConvertedAmountDict(
                spendings,
                query.TargetCurrencyId.Value,
                cancellationToken);

            result = spendings
                .Select(s => SpendingConverter.ConvertToGetSpendingsInDateRangeResponseItem(s, spendingConvertedAmountDict[s]))
                .ToArray();
        }
        else
        {
            result = spendings
                .Select(s => SpendingConverter.ConvertToGetSpendingsInDateRangeResponseItem(s))
                .ToArray();    
        }

        return result;
    }
}