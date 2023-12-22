using SpendingTracker.Application.Handlers.Spending.GetFilteredSpendings.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsWithCategories.Converters;
using SpendingTracker.Application.Handlers.Spending.Services.Abstractions;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetFilteredSpendings;

internal class GetFilteredSpendingsQueryHandler
    : QueryHandler<GetFilteredSpendingsQuery, GetFilteredSpendingsResponseItem[]>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly ISpendingsConvertingService _spendingsConvertingService;

    public GetFilteredSpendingsQueryHandler(
        ISpendingRepository spendingRepository,
        ISpendingsConvertingService spendingsConvertingService)
    {
        _spendingRepository = spendingRepository;
        _spendingsConvertingService = spendingsConvertingService;
    }

    public override async Task<GetFilteredSpendingsResponseItem[]> HandleAsync(
        GetFilteredSpendingsQuery query,
        CancellationToken cancellationToken)
    {
        var spendings = query.CategoryId.HasValue
            ? await _spendingRepository.GetUserSpendingsByCategoryInRange(
                query.CategoryId.Value,
                query.DateFrom,
                query.DateTo,
                cancellationToken)
            : await _spendingRepository.GetUserSpendingsInRange(
                query.UserId,
                query.DateFrom,
                query.DateTo,
                cancellationToken);

        GetFilteredSpendingsResponseItem[] spendingDtos;
        if (query.TargetCurrencyId.HasValue)
        {
            var spendingConvertedAmountDict = await _spendingsConvertingService.GetSpendingsConvertedAmountDict(
                spendings,
                query.TargetCurrencyId.Value,
                cancellationToken);

            spendingDtos = spendings
                .Select(spending => SpendingConverter.ConvertToGetSpendingsInDateRangeResponseItem(
                    spending,
                    spendingConvertedAmountDict[spending]))
                .ToArray();
        }
        else
        {
            spendingDtos = spendings
                .Select(s => SpendingConverter.ConvertToGetSpendingsInDateRangeResponseItem(s))
                .ToArray();    
        }
        
        var orderedDtos = spendingDtos.OrderByDescending(s => s.Amount).ToArray();
        return orderedDtos;
    }
}