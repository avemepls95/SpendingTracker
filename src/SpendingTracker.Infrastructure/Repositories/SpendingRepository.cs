using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Domain.Categories;
using SpendingTracker.Infrastructure.Abstractions.Model;
using SpendingTracker.Infrastructure.Abstractions.Model.Categories;
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
            DateTimeOffset dateFrom,
            DateTimeOffset dateTo,
            CancellationToken cancellationToken)
        {
            var dbSpendings = await _dbContext.Set<StoredSpending>()
                .Include(s => s.Currency)
                .Include(s => s.CategoryLinks)
                    .ThenInclude(l => l.Category)
                        .ThenInclude(c => c.ChildCategoryLinks)
                            .ThenInclude(l => l.Parent)
                .Where(s =>
                    !s.IsDeleted
                    && s.CreatedBy == userKey
                    && dateFrom < s.Date
                    && s.Date < dateTo)
                .ToArrayAsync(cancellationToken);
            
            var result = dbSpendings
                .Select(_spendingFactory.Create)
                .ToArray();
            
            return result;
        }

        public async Task AddExistToCategories(Guid spendingId, Category[] categories, CancellationToken cancellationToken)
        {
            var spending = await _dbContext.Set<StoredSpending>()
                .Include(s => s.CategoryLinks)
                .FirstOrDefaultAsync(s => s.Id == spendingId, cancellationToken);

            if (spending is null)
            {
                throw new ArgumentException($"Не найдена трата с идентификатором {spendingId}");
            }

            var categoryIds = categories.Select(c => c.Id).ToArray();
            var existsLinkIds = await _dbContext.Set<SpendingCategoryLink>()
                .Where(l => l.SpendingId == spendingId && categoryIds.Contains(l.SpendingId))
                .Select(l => l.CategoryId)
                .ToArrayAsync(cancellationToken);

            if (existsLinkIds.Any())
            {
                throw new ArgumentException($"Трата {spending.Id} уже входит в следующие категории: {existsLinkIds}");
            }

            var newCategoryLinks = categories.Select(c => new SpendingCategoryLink
            {
                SpendingId = spendingId,
                CategoryId = c.Id,
            });
            spending.CategoryLinks!.AddRange(newCategoryLinks);
        }
    }
}