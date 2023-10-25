using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class CategoryFactory : ICategoryFactory
{
    public Category Create(StoredCategory storedCategory, StoredSpendingCategoryLink[] userCategoryLinks = null)
    {
        var result = new Category(storedCategory.Id, storedCategory.OwnerId, storedCategory.Title);

        // if (userCategoryLinks.Any())
        // {
        //     var childCategories = storedCategory.ChildCategoryLinks.Select(l => Create(l.Child)).ToArray();
        //     result.SetChildCategories(childCategories);
        // }

        return result;
    }
}