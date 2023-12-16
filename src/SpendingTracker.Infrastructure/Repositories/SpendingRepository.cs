using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Domain.Categories;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions.Models.Request;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Repositories
{
    internal class SpendingRepository : ISpendingRepository
    {
        private readonly MainDbContext _dbContext;
        private readonly ISpendingFactory _spendingFactory;

        public SpendingRepository(MainDbContext dbContext, ISpendingFactory spendingFactory)
        {
            _dbContext = dbContext;
            _spendingFactory = spendingFactory;
        }

        public async Task CreateAsync(Spending spending, CancellationToken cancellationToken)
        {
            var storedSpending = new StoredSpending
            {
                Id = spending.Id,
                Amount = spending.Amount,
                CurrencyId = spending.Currency.Id,
                Date = spending.Date,
                Description = spending.Description,
                ActionSource = spending.ActionSource
            };

            await _dbContext.Set<StoredSpending>().AddAsync(storedSpending, cancellationToken);
        }

        public async Task<Spending[]> GetUserSpendingsInRange(
            UserKey userKey,
            DateOnly dateFrom,
            DateOnly dateTo,
            CancellationToken cancellationToken)
        {
            var datetimeForm = dateFrom.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
            var datetimeTo = dateTo.ToDateTime(TimeOnly.MaxValue).ToUniversalTime();
            var dbSpendings = await _dbContext.Set<StoredSpending>()
                .Include(s => s.Currency)
                .Where(s =>
                    !s.IsDeleted
                    && s.CreatedBy == userKey
                    && datetimeForm < s.Date
                    && s.Date < datetimeTo)
                .ToArrayAsync(cancellationToken);
            
            var spendingIds = dbSpendings.Select(s => s.Id).ToArray();

            var categoryIds = await _dbContext.Set<StoredSpendingCategoryLink>()
                .Where(l => spendingIds.Contains(l.SpendingId))
                .Select(l => l.CategoryId)
                .ToArrayAsync(cancellationToken);
            
            var result = dbSpendings
                .Select(s => _spendingFactory.Create(s, categoryIds))
                .ToArray();
            
            return result;
        }

        public async Task<Spending[]> GetUserSpendings(
            UserKey userKey,
            int offset,
            int count,
            string? searchString,
            bool onlyWithoutCategories,
            CancellationToken cancellationToken)
        {
            var queryable = _dbContext.Set<StoredSpending>()
                .Include(s => s.Currency)
                .Where(s => !s.IsDeleted && s.CreatedBy == userKey);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                queryable = queryable.Where(s => EF.Functions.ILike(s.Description, $"%{searchString}%"));
            }

            if (onlyWithoutCategories)
            {
                queryable = queryable.Where(s =>
                    !_dbContext.Set<StoredSpendingCategoryLink>()
                        .Any(l => l.SpendingId == s.Id));
            }
            
            var dbSpendings = await queryable
                .OrderByDescending(s => s.Date)
                .ThenByDescending(s => s.CreatedDate)
                .Skip(offset)
                .Take(count)
                .ToArrayAsync(cancellationToken);

            var spendingIds = dbSpendings.Select(s => s.Id).ToArray();

            var allCategoryLinks = await _dbContext.Set<StoredSpendingCategoryLink>()
                .Where(l => spendingIds.Contains(l.SpendingId))
                .ToArrayAsync(cancellationToken);
            
            var result = dbSpendings
                .Select(s =>
                {
                    var categoryIds = allCategoryLinks
                        .Where(link => link.SpendingId == s.Id)
                        .Select(link => link.CategoryId)
                        .ToArray();
                        
                    return _spendingFactory.Create(s, categoryIds);
                })
                .ToArray();
            
            return result;
        }

        public async Task<Spending> GetUserSpendingById(Guid id, CancellationToken cancellationToken)
        {
            var dbSpending = await _dbContext.Set<StoredSpending>()
                .Include(s => s.Currency)
                .Where(s => !s.IsDeleted && s.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbSpending is null)
            {
                throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.KeyNotFound);
            }
            
            var categoryIds = await _dbContext.Set<StoredSpendingCategoryLink>()
                .Where(l => l.SpendingId == id)
                .Select(l => l.CategoryId)
                .ToArrayAsync(cancellationToken);

            var result = _spendingFactory.Create(dbSpending, categoryIds);
            return result;
        }

        public async Task AddExistToCategories(Guid spendingId, Category[] categories, CancellationToken cancellationToken)
        {
            var spending = await _dbContext.Set<StoredSpending>()
                .FirstOrDefaultAsync(s => s.Id == spendingId, cancellationToken);

            if (spending is null)
            {
                throw new ArgumentException($"Не найдена трата с идентификатором {spendingId}");
            }

            var categoryIds = categories.Select(c => c.Id).ToArray();
            var existsLinkIds = await _dbContext.Set<StoredSpendingCategoryLink>()
                .Where(l => l.SpendingId == spendingId && categoryIds.Contains(l.SpendingId))
                .Select(l => l.CategoryId)
                .ToArrayAsync(cancellationToken);

            if (existsLinkIds.Any())
            {
                throw new ArgumentException($"Трата {spending.Id} уже входит в следующие категории: {existsLinkIds}");
            }

            var newCategoryLinks = categories.Select(c => new StoredSpendingCategoryLink
            {
                SpendingId = spendingId,
                CategoryId = c.Id,
            });
            await _dbContext.Set<StoredSpendingCategoryLink>().AddRangeAsync(newCategoryLinks, cancellationToken);
        }

        public async Task DeleteLastUserSpending(UserKey userId, CancellationToken cancellationToken)
        {
            var lastUserSpending = await _dbContext.Set<StoredSpending>()
                .Where(s => !s.IsDeleted && s.CreatedBy == userId)
                .OrderByDescending(s => s.CreatedDate)
                .FirstAsync(cancellationToken);

            lastUserSpending.IsDeleted = true;
        }

        public async Task DeleteSpending(Guid spendingId, CancellationToken cancellationToken)
        {
            var lastUserSpending = await _dbContext.Set<StoredSpending>()
                .Where(s => !s.IsDeleted && s.Id == spendingId)
                .FirstOrDefaultAsync(cancellationToken);

            if (lastUserSpending == null)
            {
                throw new ArgumentException($"Не найдена трата с идентификатором {spendingId}");
            }

            lastUserSpending.IsDeleted = true;
        }

        public async Task UpdateSpending(UpdateSpendingModel model, CancellationToken cancellationToken)
        {
            var lastUserSpending = await _dbContext.Set<StoredSpending>()
                .Where(s => !s.IsDeleted && s.Id == model.Id)
                .OrderByDescending(s => s.CreatedDate)
                .FirstAsync(cancellationToken);
            
            if (lastUserSpending == null)
            {
                throw new ArgumentException($"Не найдена трата с идентификатором {model.Id}");
            }

            lastUserSpending.CurrencyId = model.CurrencyId;
            lastUserSpending.Amount = model.Amount;
            lastUserSpending.Description = model.Description;
            lastUserSpending.Date = model.Date;
        }

        public async Task<Spending> GetById(Guid id, CancellationToken cancellationToken)
        {
            var dbSpending = await _dbContext.Set<StoredSpending>().FirstOrDefaultAsync(
                s => s.Id == id&& !s.IsDeleted,
                cancellationToken);

            if (dbSpending is null)
            {
                throw new KeyNotFoundException($"Не найдена трата с идентификатором {id}");
            }
            
            var categoryIds = await _dbContext.Set<StoredSpendingCategoryLink>()
                .Where(l => l.SpendingId == id)
                .Select(l => l.CategoryId)
                .ToArrayAsync(cancellationToken);

            return _spendingFactory.Create(dbSpending, categoryIds);
        }

        public async Task AddToCategory(
            Guid spendingId,
            Guid categoryId,
            CancellationToken cancellationToken)
        {
            var isSpendingExists = await _dbContext.Set<StoredSpending>().AnyAsync(
                s => s.Id == spendingId,
                cancellationToken);

            if (!isSpendingExists)
            {
                throw new KeyNotFoundException($"Не найдена трата с идентификатором {spendingId}");
            }
            
            var isCategoryExists = _dbContext.Set<StoredCategory>().Local.Any(s => s.Id == categoryId);
            if (!isCategoryExists)
            {
                isCategoryExists = await _dbContext.Set<StoredCategory>().AnyAsync(
                    s => s.Id == categoryId,
                    cancellationToken);

                if (!isCategoryExists)
                {
                    throw new KeyNotFoundException($"Не найдена категория с идентификатором {categoryId}");
                }
            }

            var link = new StoredSpendingCategoryLink
            {
                SpendingId = spendingId,
                CategoryId = categoryId
            };

            await _dbContext.Set<StoredSpendingCategoryLink>().AddAsync(link, cancellationToken);
        }

        public async Task RemoveFromCategory(Guid spendingId, Guid categoryId, CancellationToken cancellationToken)
        {
            var isLinkExists = await _dbContext.Set<StoredSpendingCategoryLink>().AnyAsync(
                s => s.SpendingId == spendingId && s.CategoryId == categoryId,
                cancellationToken);

            if (!isLinkExists)
            {
                throw new KeyNotFoundException($"Трата {spendingId} не входит в категорию {categoryId}");
            }

            var link = new StoredSpendingCategoryLink
            {
                SpendingId = spendingId,
                CategoryId = categoryId,
            };
            _dbContext.Set<StoredSpendingCategoryLink>().Attach(link);

            _dbContext.Set<StoredSpendingCategoryLink>().Remove(link);
        }

        public Task<bool> IsSpendingHasCategory(
            Guid spendingId,
            Guid categoryId,
            CancellationToken cancellationToken)
        {
            return _dbContext.Set<StoredSpendingCategoryLink>().AnyAsync(
                l => l.SpendingId == spendingId && l.CategoryId == categoryId,
                cancellationToken);
        }
    }
}