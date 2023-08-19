using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Model.Categories;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

public interface ICategoryFactory
{
    Category Create(StoredCategory storedCategory);
}