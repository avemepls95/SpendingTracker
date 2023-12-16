using MediatR;
using SpendingTracker.Application.Handlers.Analytics.GetCategoriesAnalyticsByRange.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Analytics;

internal sealed class GetCategoriesAnalyticsByRangeQueryHandler
    : QueryHandler<GetCategoriesAnalyticsByRangeQuery, GetCategoriesAnalyticsByRangeResponse>
{
    private readonly IMediator _mediator;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISpendingCategoryLinksRepository _spendingCategoryLinksRepository;
    private readonly ICurrencyRepository _currencyRepository;

    public GetCategoriesAnalyticsByRangeQueryHandler(
        IMediator mediator,
        ICategoryRepository categoryRepository,
        ISpendingCategoryLinksRepository spendingCategoryLinksRepository,
        ICurrencyRepository currencyRepository)
    {
        _mediator = mediator;
        _categoryRepository = categoryRepository;
        _spendingCategoryLinksRepository = spendingCategoryLinksRepository;
        _currencyRepository = currencyRepository;
    }

    public override async Task<GetCategoriesAnalyticsByRangeResponse> HandleAsync(
        GetCategoriesAnalyticsByRangeQuery query,
        CancellationToken cancellationToken)
    {
        var currencyExists = await _currencyRepository.IsExistsById(query.TargetCurrencyId, cancellationToken);
        if (!currencyExists)
        {
            throw new ArgumentException("Некорректное значение валюты.");
        }
        
        var spendings =
            await _mediator.SendQueryAsync<GetSpendingsInDateRangeQuery, GetSpendingsInDateRangeResponseItem[]>(
                new GetSpendingsInDateRangeQuery
                {
                    UserId = query.UserId,
                    DateFrom = query.DateFrom,
                    DateTo = query.DateTo,
                    TargetCurrencyId = query.TargetCurrencyId
                },
                cancellationToken);

        if (spendings.Length == 0)
        {
            return new GetCategoriesAnalyticsByRangeResponse();
        }
        
        var categoriesTree = await _categoryRepository.GetUserCategoriesTree(query.UserId, cancellationToken);

        var spendingIds = spendings.Select(s => s.Id).ToArray();
        var spendingCategoryLinks = await _spendingCategoryLinksRepository.GetBySpendings(
            spendingIds,
            cancellationToken);

        // Формируем развернутый список выходных объектов на основе дерева категорий
        var expandedTreeDtos = new List<GetCategoriesAnalyticsByRangeResponseItem>();
        foreach (var rootCategoryNode in categoriesTree)
        {
            var branchWithExpandedDtos = ConvertBranchToExpandedBranchDtos(rootCategoryNode);
            expandedTreeDtos.AddRange(branchWithExpandedDtos);

            var categoryIdsWithSpendings = branchWithExpandedDtos
                .Select(d => d.CategoryId)
                .Where(id => spendingCategoryLinks.Select(l => l.CategoryId).Contains(id))
                .ToArray();

            // Находим категории с тратами, чтобы вычислить их траты и заполнить их в родительские категории
            var expandedBranch = ExpandTreeBranch(rootCategoryNode);
            var categoriesWithSpendings = expandedBranch
                .Where(c => categoryIdsWithSpendings.Contains(c.Id))
                .ToArray();

            foreach (var categoryWithSpendings in categoriesWithSpendings)
            {
                var spendingsIds = spendingCategoryLinks
                    .Where(l => l.CategoryId == categoryWithSpendings.Id)
                    .Select(l => l.SpendingId)
                    .ToArray();

                var amountInCategoryWithSpendings = spendings.Where(s => spendingsIds.Contains(s.Id)).Sum(s => s.Amount);

                // К категории с тратами и всем ее родителям добавляем сумму трат
                FillAmountInParentsFromChild(amountInCategoryWithSpendings, categoryWithSpendings, branchWithExpandedDtos);
            }
        }

        // В развернутом дереве оставляем только корни
        var resultCategoryInfos = expandedTreeDtos
            .Where(d =>
                d.ParentIds.Count == 0
                && d.Amount != 0)
            .OrderByDescending(c => c.Amount)
            .ToArray();

        var totalAmount = spendings.Sum(s => s.Amount);
        return new GetCategoriesAnalyticsByRangeResponse
        {
            TotalAmount = totalAmount,
            CategoryInfos = resultCategoryInfos
        };
    }

    private GetCategoriesAnalyticsByRangeResponseItem[] ConvertBranchToExpandedBranchDtos(
        Domain.Categories.Category rootCategory)
    {
        var result = new List<GetCategoriesAnalyticsByRangeResponseItem>();
        var rootDto = new GetCategoriesAnalyticsByRangeResponseItem
        {
            CategoryId = rootCategory.Id,
            CategoryTitle = rootCategory.Title
        };
        result.Add(rootDto);

        FillChildsDtoForParentDto(rootCategory, rootDto);

        return result.ToArray();

        void FillChildsDtoForParentDto(
            Domain.Categories.Category parent,
            GetCategoriesAnalyticsByRangeResponseItem parentDto)
        {
            foreach (var child in parent.Childs)
            {
                var childDto = new GetCategoriesAnalyticsByRangeResponseItem
                {
                    CategoryId = child.Id,
                    CategoryTitle = child.Title
                };
                childDto.ParentIds.Add(parent.Id);
                parentDto.Childs.Add(childDto);
                result.Add(childDto);
                
                FillChildsDtoForParentDto(child, childDto);
            }
        }
    }

    private Domain.Categories.Category[] ExpandTreeBranch(Domain.Categories.Category root)
    {
        var result = new List<Domain.Categories.Category>();
        AddChilds(root);

        return result.ToArray();

        void AddChilds(Domain.Categories.Category parent)
        {
            result.Add(parent);
            foreach (var child in parent.Childs)
            {
                AddChilds(child);
            }
        }
    }

    private void FillAmountInParentsFromChild(
        double amount,
        Domain.Categories.Category child,
        GetCategoriesAnalyticsByRangeResponseItem[] expandedTreeDtos)
    {
        var childDto = expandedTreeDtos.First(d => d.CategoryId == child.Id);
        childDto.Amount += amount;
        foreach (var parent in child.Parents)
        {
            FillAmountInParentsFromChild(amount, parent, expandedTreeDtos);
        }
    }
}