using SpendingTracker.Application.Factories.Abstractions;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Categories;

namespace SpendingTracker.Application.Factories;

internal class CategoryFactory : ICategoryFactory
{
    public Category Create(string title, UserKey userId)
    {
        return new Category(Guid.NewGuid(), userId, title);
    }
}