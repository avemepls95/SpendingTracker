using SpendingTracker.Application.Handlers.Spending.GetSpendingsWithCategories.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsWithCategories.Converters;
using SpendingTracker.Application.Handlers.Spending.Services.Abstractions;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsWithCategories;

internal class GetSpendingsWithCategoriesQueryHandler
    : QueryHandler<GetSpendingsWithCategoriesQuery, GetSpendingsWithCategoriesResponseItem[]>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISpendingsConvertingService _spendingsConvertingService;

    public GetSpendingsWithCategoriesQueryHandler(
        ISpendingRepository spendingRepository,
        ICategoryRepository categoryRepository,
        ISpendingsConvertingService spendingsConvertingService)
    {
        _spendingRepository = spendingRepository;
        _categoryRepository = categoryRepository;
        _spendingsConvertingService = spendingsConvertingService;
    }

    public override async Task<GetSpendingsWithCategoriesResponseItem[]> HandleAsync(
        GetSpendingsWithCategoriesQuery withCategoriesQuery,
        CancellationToken cancellationToken)
    {
        var spendings = await _spendingRepository.GetUserSpendings(
            withCategoriesQuery.UserId,
            withCategoriesQuery.Offset,
            withCategoriesQuery.Count,
            withCategoriesQuery.SearchString,
            withCategoriesQuery.OnlyWithoutCategories,
            cancellationToken);

        var categoriesTree = await _categoryRepository.GetUserCategoriesReverseTree(
            withCategoriesQuery.UserId,
            cancellationToken);

        GetSpendingsWithCategoriesResponseItem[] result; 
        if (withCategoriesQuery.TargetCurrencyId.HasValue)
        {
            var spendingConvertedAmountDict = await _spendingsConvertingService.GetSpendingsConvertedAmountDict(
                spendings,
                withCategoriesQuery.TargetCurrencyId.Value,
                cancellationToken);

            result = spendings
                .Select(spending => SpendingConverter.ConvertToGetSpendingsResponseItem(
                    spending,
                    spendingConvertedAmountDict[spending],
                    categoriesTree))
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