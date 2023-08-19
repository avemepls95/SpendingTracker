using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;
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

        public async Task<Spending[]> GetUserSpendings(
            UserKey userKey,
            DateTimeOffset? dateFrom,
            DateTimeOffset? dateTo,
            CancellationToken cancellationToken)
        {
            var queryable = _dbContext.Set<StoredSpending>()
                .Include(s => s.Currency)
                .Include(s => s.CategoryLinks)
                    .ThenInclude(l => l.Category)
                        .ThenInclude(c => c.ParentCategoryLinks)
                .Where(s => !s.IsDeleted && s.CreatedBy == userKey);

            if (dateFrom.HasValue)
            {
                queryable = queryable.Where(s => dateFrom < s.Date);
            }
            
            if (dateTo.HasValue)
            {
                queryable = queryable.Where(s => s.Date < dateTo);
            }

            var dbSpendings = await queryable.ToArrayAsync(cancellationToken);
            
            var result = dbSpendings
                .Select(_spendingFactory.Create)
                .ToArray();
            
            return result;
        }
    }
}