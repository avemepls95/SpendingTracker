using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Infrastructure.Repositories
{
    public class SpendingRepository : ISpendingRepository
    {
        private readonly MainDbContext _dbContext;

        public SpendingRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Spending ticket, CancellationToken cancellationToken)
        {
            await _dbContext.Set<Spending>().AddAsync(ticket, cancellationToken);
        }
    }
}