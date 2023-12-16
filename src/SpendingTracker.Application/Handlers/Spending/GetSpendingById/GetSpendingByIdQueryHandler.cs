using SpendingTracker.Application.Handlers.Spending.GetSpendingById.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendings.Converters;
using SpendingTracker.Application.Handlers.Spending.Services.Abstractions;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingById;

internal class GetSpendingByIdQueryHandler : QueryHandler<GetSpendingByIdQuery, GetSpendingByIdResponse>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISpendingsConvertingService _spendingsConvertingService;

    public GetSpendingByIdQueryHandler(
        ISpendingRepository spendingRepository,
        ICategoryRepository categoryRepository,
        ISpendingsConvertingService spendingsConvertingService)
    {
        _spendingRepository = spendingRepository;
        _categoryRepository = categoryRepository;
        _spendingsConvertingService = spendingsConvertingService;
    }

    public override async Task<GetSpendingByIdResponse> HandleAsync(
        GetSpendingByIdQuery query,
        CancellationToken cancellationToken)
    {
        var spending = await _spendingRepository.GetUserSpendingById(query.Id, cancellationToken);
        
        var categoriesTree = await _categoryRepository.GetSpendingCategoriesReverseTree(query.Id, cancellationToken);

        GetSpendingByIdResponse result; 
        if (query.TargetCurrencyId.HasValue)
        {
            var spendingConvertedAmountDict = await _spendingsConvertingService.GetSpendingsConvertedAmountDict(
                new [] { spending },
                query.TargetCurrencyId.Value,
                cancellationToken);

            result = SpendingConverter.ConvertToGetSpendingByIdResponse(spending, spendingConvertedAmountDict[spending], categoriesTree);
        }
        else
        {
            result = SpendingConverter.ConvertToGetSpendingByIdResponse(spending, categoriesTree);
        }

        return result;
    }
}