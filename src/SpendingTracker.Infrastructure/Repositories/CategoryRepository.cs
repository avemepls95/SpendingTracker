using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Repositories;

internal class CategoryRepository : ICategoryRepository
{
    private readonly MainDbContext _dbContext;
    private readonly ICategoryFactory _categoryFactory;

    public CategoryRepository(MainDbContext dbContext, ICategoryFactory categoryFactory)
    {
        _dbContext = dbContext;
        _categoryFactory = categoryFactory;
    }

    public async Task CreateCategory(Category newCategory, CancellationToken cancellationToken)
    {
        var newStoredCategory = new StoredCategory
        {
            Id = newCategory.Id,
            OwnerId = newCategory.OwnerId,
            Title = newCategory.Title
        };

        await _dbContext.Set<StoredCategory>().AddAsync(newStoredCategory, cancellationToken);
    }

    public async Task CreateCategories(Category[] newCategories, CancellationToken cancellationToken)
    {
        if (newCategories is null || newCategories.Length == 0)
        {
            throw new ArgumentNullException(nameof(newCategories));
        }

        var ownerId = newCategories.First().OwnerId;
        if (newCategories.Any(c => c.OwnerId != ownerId))
        {
            throw new ArgumentException("Переданы категории с разными владельцами");
        }
        
        var newCategoriesTitles = newCategories.Select(c => c.Title.ToUpper()).ToArray();
        var existUserCategories = await _dbContext.Set<StoredCategory>()
            .Where(c => c.OwnerId == ownerId && newCategoriesTitles.Contains(c.Title.ToUpper()))
            .ToArrayAsync(cancellationToken);

        if (existUserCategories.Any())
        {
            var existUserCategoriesTitles = string.Join(", ", existUserCategories.Select(c => $"\"{c.Title}\"")); 
            throw new ArgumentException($"Уже существуют категории с названиями: {existUserCategoriesTitles}");
        }
        
        var newStoredCategories = newCategories.Select(c => new StoredCategory
        {
            Id = c.Id,
            OwnerId = c.OwnerId,
            Title = c.Title
        }).ToArray();
        
        await _dbContext.Set<StoredCategory>().AddRangeAsync(newStoredCategories, cancellationToken);
    }

    public async Task DeleteCategory(Category category, CancellationToken cancellationToken)
    {
        var categoriesSet = _dbContext.Set<StoredCategory>();
        var dbCategory = categoriesSet.Local.FirstOrDefault(c =>
            c.Id == category.Id
            && !c.IsDeleted);

        if (dbCategory is null)
        {
            dbCategory = await categoriesSet.FirstOrDefaultAsync(
                c => c.Id == category.Id && !c.IsDeleted,
                cancellationToken);

            if (dbCategory is null)
            {
                throw new KeyNotFoundException($"Не найдена категория с идентификатором {category.Id}");
            }
        }

        dbCategory.IsDeleted = true;
    }

    public async Task AddChildCategory(Category parent, Category child, CancellationToken cancellationToken)
    {
        if (parent is null)
        {
            throw new ArgumentNullException(nameof(parent));
        }
        
        await AddChildCategoryInternal(parent.Id, child.Id, cancellationToken);
    }

    public async Task AddChildrenCategories(Category parent, Category[] children, CancellationToken cancellationToken)
    {
        if (parent is null)
        {
            throw new ArgumentNullException(nameof(parent));
        }

        foreach (var child in children)
        {
            await AddChildCategoryInternal(parent.Id, child.Id, cancellationToken);
        }
    }

    public async Task AddChildCategory(Guid parentId, Guid childId, CancellationToken cancellationToken)
    {
        var parentCategory = await _dbContext.Set<StoredCategory>().FirstOrDefaultAsync(
            c => c.Id == parentId,
            cancellationToken);

        if (parentCategory is null)
        {
            throw new ArgumentException($"Родительская категория с идентификатором {parentId} не найдена");
        }

        await AddChildCategoryInternal(parentId, childId, cancellationToken);
    }

    private async Task AddChildCategoryInternal(Guid parentId, Guid childId, CancellationToken cancellationToken)
    {
        var linkAlreadyExists = await _dbContext.Set<CategoriesLink>().AnyAsync(
            l => l.ParentId == parentId && l.ChildId == childId,
            cancellationToken);

        if (linkAlreadyExists)
        {
            throw new ArgumentException($"У категории {parentId} уже есть дочерняя категория {childId}");
        }

        var categoriesLink = new CategoriesLink
        {
            ChildId = childId,
            ParentId = parentId
        };
        await _dbContext.Set<CategoriesLink>().AddAsync(categoriesLink, cancellationToken);
    }

    public async Task<Category> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dbCategory = await _dbContext.Set<StoredCategory>().FirstOrDefaultAsync(
            c => c.Id == id && !c.IsDeleted,
            cancellationToken);

        if (dbCategory is null)
        {
            throw new KeyNotFoundException($"Категория с идентификатором {id} не найдена");
        }

        var result = _categoryFactory.Create(dbCategory);
        return result;
    }

    public async Task<Category[]> GetByIds(Guid[] ids, CancellationToken cancellationToken)
    {
        var dbCategories = await _dbContext.Set<StoredCategory>()
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id) && !c.IsDeleted)
            .ToArrayAsync(cancellationToken);

        if (dbCategories.Length != ids.Length)
        {
            var notFoundCategoryIds = ids.Except(dbCategories.Select(c => c.Id)).ToArray();
            var notFoundCategoryIdsAsString = string.Join(" ,", notFoundCategoryIds);
            throw new KeyNotFoundException($"Некоторые категория не найдены. Идентификаторы: {notFoundCategoryIdsAsString}");
        }

        var result = dbCategories.Select(_categoryFactory.Create).ToArray();
        return result;
    }

    public async Task<Category[]> GetUserCategories(UserKey userId, CancellationToken cancellationToken)
    {
        var dbCategories = await _dbContext.Set<StoredCategory>()
            .Where(c => c.OwnerId == userId && !c.IsDeleted)
            .ToArrayAsync(cancellationToken);

        var result = dbCategories.Select(_categoryFactory.Create).ToArray();
        return result;
    }
}