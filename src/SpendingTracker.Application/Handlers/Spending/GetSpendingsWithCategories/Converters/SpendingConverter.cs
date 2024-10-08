﻿using SpendingTracker.Application.Handlers.Spending.Common;
using SpendingTracker.Application.Handlers.Spending.GetFilteredSpendings.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendingById.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsWithCategories.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendingsWithCategories.Converters;

public static class SpendingConverter
{
    public static GetSpendingsWithCategoriesResponseItem ConvertToGetSpendingsResponseItem(
        Domain.Spending spending,
        Domain.Categories.Category[] categoriesTree)
    {
        var categories = ConvertCategories(spending, categoriesTree);
        
        return new GetSpendingsWithCategoriesResponseItem
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
    
    public static GetFilteredSpendingsResponseItem ConvertToGetSpendingsInDateRangeResponseItem(Domain.Spending spending)
    {
        return new GetFilteredSpendingsResponseItem
        {
            Id = spending.Id,
            Amount = spending.Amount,
            CurrencyId = spending.Currency.Id,
            Date = spending.Date,
            Description = spending.Description,
            CreateDate = spending.CreatedDate
        };
    }
    
    public static GetFilteredSpendingsResponseItem ConvertToGetSpendingsInDateRangeResponseItem(
        Domain.Spending spending,
        double customCurrencyAmount)
    {
        return new GetFilteredSpendingsResponseItem
        {
            Id = spending.Id,
            Amount = customCurrencyAmount,
            CurrencyId = spending.Currency.Id,
            Date = spending.Date,
            Description = spending.Description,
            CreateDate = spending.CreatedDate
        };
    }
    
    public static GetSpendingsWithCategoriesResponseItem ConvertToGetSpendingsResponseItem(
        Domain.Spending spending,
        double customCurrencyAmount,
        Domain.Categories.Category[] categoriesTree)
    {
        var categories = ConvertCategories(spending, categoriesTree);
        
        return new GetSpendingsWithCategoriesResponseItem
        {
            Id = spending.Id,
            Amount = spending.Amount,
            CurrencyId = spending.Currency.Id,
            Date = spending.Date,
            Description = spending.Description,
            CreateDate = spending.CreatedDate,
            Categories = categories,
            CustomCurrencyAmount = customCurrencyAmount
        };
    }
    
    public static GetSpendingByIdResponse ConvertToGetSpendingByIdResponse(
        Domain.Spending spending,
        Domain.Categories.Category[] categoriesTree)
    {
        var categories = ConvertCategories(spending, categoriesTree);
        
        return new GetSpendingByIdResponse
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
    
    public static GetSpendingByIdResponse ConvertToGetSpendingByIdResponse(
        Domain.Spending spending,
        double customCurrencyAmount,
        Domain.Categories.Category[] categoriesTree)
    {
        var categories = ConvertCategories(spending, categoriesTree);
        
        return new GetSpendingByIdResponse
        {
            Id = spending.Id,
            Amount = spending.Amount,
            CurrencyId = spending.Currency.Id,
            Date = spending.Date,
            Description = spending.Description,
            CreateDate = spending.CreatedDate,
            Categories = categories,
            CustomCurrencyAmount = customCurrencyAmount
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