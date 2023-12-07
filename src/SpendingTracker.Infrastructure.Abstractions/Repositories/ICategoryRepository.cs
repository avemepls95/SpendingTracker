using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Models.Request;

namespace SpendingTracker.Infrastructure.Abstractions.Repositories;

public interface ICategoryRepository
{
    Task CreateCategory(Category newCategory, CancellationToken cancellationToken = default);
    Task UpdateCategory(UpdateCategoryModel updateModel, CancellationToken cancellationToken = default);
    Task CreateCategories(Category[] newCategories, CancellationToken cancellationToken = default);
    Task DeleteCategory(Category category, CancellationToken cancellationToken = default);
    Task AddChildCategory(Category parent, Category child, CancellationToken cancellationToken = default);
    Task AddChildCategory(Guid parentId, Guid childId, CancellationToken cancellationToken = default);
    Task<Category> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Category[]> GetByIds(Guid[] ids, CancellationToken cancellationToken = default);
    Task<Category[]> GetUserCategories(UserKey userId, CancellationToken cancellationToken = default);
    Task<Category[]> GetUserCategoriesTree(UserKey userId, CancellationToken cancellationToken = default);
    Task<Category[]> GetSpendingCategoriesTree(Guid spendingId, CancellationToken cancellationToken = default);
    Task<bool> UserHasByTitle(UserKey userId, string title, CancellationToken cancellationToken = default);
    Task<bool> UserHasById(UserKey userId, Guid categoryId, CancellationToken cancellationToken = default);
    Task RemoveChildFromParent(Guid childId, Guid parentId, CancellationToken cancellationToken = default);
    void RemoveAllLinksWithAnotherCategories(Guid id);
    void RemoveFromAllSpendings(Guid id);
}