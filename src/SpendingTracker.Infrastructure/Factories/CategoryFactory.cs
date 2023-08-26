using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Model.Categories;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class CategoryFactory : ICategoryFactory
{
    public Category Create(StoredCategory storedCategory)
    {
        var result = new Category(storedCategory.Id, storedCategory.OwnerId, storedCategory.Title);

        if (storedCategory.ChildCategoryLinks.Any())
        {
            var childCategories = storedCategory.ChildCategoryLinks.Select(l => Create(l.Child)).ToArray();
            result.SetChildCategories(childCategories);
        }
        
        if (storedCategory.ParentCategoryLinks.Any())
        {
            var parentCategories = storedCategory.ParentCategoryLinks.Select(l => Create(l.Parent)).ToArray();
            result.SetParentCategories(parentCategories);
        }

        return result;
    }
}