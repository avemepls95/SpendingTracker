using SpendingTracker.Domain.Categories;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface ICategoryRepository
{
    Task CreateCategory(Category newCategory, CancellationToken cancellationToken = default);
    Task CreateCategories(Category[] newCategories, CancellationToken cancellationToken = default);
    Task AddChildCategory(Guid parentId, Guid childId, CancellationToken cancellationToken = default);
}