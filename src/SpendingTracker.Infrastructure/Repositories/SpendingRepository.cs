using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Infrastructure.Repositories
{
    internal class SpendingRepository : ISpendingRepository
    {
        private readonly MainDbContext _dbContext;

        public SpendingRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}