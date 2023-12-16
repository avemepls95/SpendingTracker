using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendings.Converters;
using SpendingTracker.Application.Handlers.Spending.Services.Abstractions;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendings;

internal class GetSpendingsQueryHandler : QueryHandler<GetSpendingsQuery, GetSpendingsResponseItem[]>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISpendingsConvertingService _spendingsConvertingService;

    public GetSpendingsQueryHandler(
        ISpendingRepository spendingRepository,
        ICategoryRepository categoryRepository,
        ISpendingsConvertingService spendingsConvertingService)
    {
        _spendingRepository = spendingRepository;
        _categoryRepository = categoryRepository;
        _spendingsConvertingService = spendingsConvertingService;
    }

    public override async Task<GetSpendingsResponseItem[]> HandleAsync(
        GetSpendingsQuery query,
        CancellationToken cancellationToken)
    {
        var spendings = await _spendingRepository.GetUserSpendings(
            query.UserId,
            query.Offset,
            query.Count,
            query.SearchString,
            query.OnlyWithoutCategories,
            cancellationToken);

        var categoriesTree = await _categoryRepository.GetUserCategoriesReverseTree(query.UserId, cancellationToken);

        GetSpendingsResponseItem[] result; 
        if (query.TargetCurrencyId.HasValue)
        {
            var spendingConvertedAmountDict = await _spendingsConvertingService.GetSpendingsConvertedAmountDict(
                spendings,
                query.TargetCurrencyId.Value,
                cancellationToken);

            result = spendings
                .Select(s => SpendingConverter.ConvertToGetSpendingsResponseItem(s, spendingConvertedAmountDict[s], categoriesTree))
                .ToArray();
        }
        else
        {
            result = spendings
                .Select(s => SpendingConverter.ConvertToGetSpendingsResponseItem(s, categoriesTree))
                .ToArray();
        }

        return result;
    }

    
}