using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendings.Converters;

public static class SpendingConverter
{
    public static GetSpendingsResponseItem ConvertToDto(
        Domain.Spending spending,
        Domain.Categories.Category[] categoriesTree)
    {
        var categories = ConvertCategories(spending, categoriesTree);
        
        return new GetSpendingsResponseItem
        {
            Id = spending.Id,
            Amount = spending.Amount,
            CurrencyId = spending.Currency.Id,
            Date = spending.Date,
            Description = spending.Description,
            CreateDate = spending.CreatedDate,
            Categories = categories
        };
    }

    private static CategoryDto[] ConvertCategories(
        Domain.Spending spending,
        Domain.Categories.Category[] categoriesTree)
    {
        var firstLevelCategories = categoriesTree
            .Where(c => spending.CategoryIds.Contains(c.Id))
            .ToArray();

        var result = new List<CategoryDto>();
        foreach (var firstLevelCategory in firstLevelCategories)
        {
            var categoryDto = new CategoryDto
            {
                Id = firstLevelCategory.Id,
                Title = firstLevelCategory.Title,
            };
            
            result.Add(categoryDto);
            ProcessCategory(firstLevelCategory, categoryDto);
        }

        return result.ToArray();

        void ProcessCategory(Domain.Categories.Category category, CategoryDto categoryDto)
        {
            categoryDto.Parents = category.Parents.Select(c => new CategoryDto
            {
                Id = c.Id,
                Title = c.Title
            }).ToArray();

            foreach (var parent in category.Parents)
            {
                var parentDto = categoryDto.Parents.First(p => p.Id == parent.Id);
                ProcessCategory(parent, parentDto);
            }
        }
    }
}