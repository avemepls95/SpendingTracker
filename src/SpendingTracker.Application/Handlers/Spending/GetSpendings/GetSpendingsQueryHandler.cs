using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendings.Converters;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendings;

internal class GetSpendingsQueryHandler : QueryHandler<GetSpendingsQuery, GetSpendingsResponseItem[]>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetSpendingsQueryHandler(ISpendingRepository spendingRepository, ICategoryRepository categoryRepository)
    {
        _spendingRepository = spendingRepository;
        _categoryRepository = categoryRepository;
    }

    public override async Task<GetSpendingsResponseItem[]> HandleAsync(
        GetSpendingsQuery query,
        CancellationToken cancellationToken)
    {
        var spendings = await _spendingRepository.GetUserSpendings(
            query.UserId,
            query.Offset,
            query.Count,
            cancellationToken);

        var categoriesTree = await _categoryRepository.GetUserCategoriesTree(query.UserId, cancellationToken);

        var result = spendings
            .Select(s => SpendingConverter.ConvertToDto(s, categoriesTree))
            .ToArray();

        return result;
    }
}