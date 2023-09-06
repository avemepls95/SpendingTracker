using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Categories;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface ICategoryRepository
{
    Task CreateCategory(Category newCategory, CancellationToken cancellationToken = default);
    Task CreateCategories(Category[] newCategories, CancellationToken cancellationToken = default);
    Task DeleteCategory(Category category, CancellationToken cancellationToken = default);
    Task AddChildCategory(Category parent, Category child, CancellationToken cancellationToken = default);
    Task AddChildrenCategories(Category parent, Category[] children, CancellationToken cancellationToken = default);
    Task AddChildCategory(Guid parentId, Guid childId, CancellationToken cancellationToken = default);
    Task<Category> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Category[]> GetByIds(Guid[] ids, CancellationToken cancellationToken = default);
    Task<Category[]> GetUserCategories(UserKey userId, CancellationToken cancellationToken = default);
}