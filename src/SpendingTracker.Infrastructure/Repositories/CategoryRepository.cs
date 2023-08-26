using Microsoft.EntityFrameworkCore;
using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Model.Categories;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Infrastructure.Repositories;

internal class CategoryRepository : ICategoryRepository
{
    private readonly MainDbContext _dbContext;

    public CategoryRepository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
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

    public async Task AddChildCategory(Guid parentId, Guid childId, CancellationToken cancellationToken)
    {
        var parentCategory = await _dbContext.Set<StoredCategory>().FirstOrDefaultAsync(
            c => c.Id == parentId,
            cancellationToken);

        if (parentCategory is null)
        {
            throw new ArgumentException($"Родительская категория с идентификатором {parentId} не найдена");
        }

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
}