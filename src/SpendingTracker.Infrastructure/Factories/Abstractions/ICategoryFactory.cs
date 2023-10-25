using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

internal interface ICategoryFactory
{
    Category Create(StoredCategory storedCategory, StoredSpendingCategoryLink[] userCategoryLinks = null);
}