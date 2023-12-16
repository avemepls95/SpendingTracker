using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class CategoryFactory : ICategoryFactory
{
    public Category Create(StoredCategory storedCategory)
    {
        var result = new Category(storedCategory.Id, storedCategory.OwnerId, storedCategory.Title);

        return result;
    }
}