using SpendingTracker.Application.Handlers.Category.GetUserCategories.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.GetUserCategories;

internal sealed class GetUserCategoriesQueryHandler : QueryHandler<GetUserCategoriesQuery, GetUserCategoriesResponseItem[]>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetUserCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public override async Task<GetUserCategoriesResponseItem[]> HandleAsync(
        GetUserCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetUserCategories(query.UserId, cancellationToken);
        var result = categories.Select(c => new GetUserCategoriesResponseItem
        {
            Id = c.Id,
            CreateDate = c.CreatedDate,
            Title = c.Title
        }).ToArray();

        return result;
    }
}