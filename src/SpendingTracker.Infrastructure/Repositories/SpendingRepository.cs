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

        public async Task CreateAsync(StoredSpending spending, CancellationToken cancellationToken)
        {
            await _dbContext.Set<StoredSpending>().AddAsync(spending, cancellationToken);
        }
    }
}