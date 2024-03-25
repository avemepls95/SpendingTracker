using Microsoft.EntityFrameworkCore;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Infrastructure.Abstractions.Models.Request;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Infrastructure.Repositories;

internal class IncomeRepository : IIncomeRepository
{
    private readonly MainDbContext _dbContext;
    // private readonly IIncomeFactory _incomeFactory;

    public IncomeRepository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // public async Task<Income[]> GetOrderedUserIncomes(UserKey userId, CancellationToken cancellationToken)
    // {
    //     var dbIncomes = await _dbContext.Set<StoredIncome>()
    //         .Where(i => i.CreatedBy == userId && !i.IsDeleted)
    //         .ToArrayAsync(cancellationToken);
    //     
    //     var result = dbIncomes
    //         .OrderBy(i => i.)
    // }

    public async Task Create(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        var newStoredIncome = new StoredIncome
        {
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            AccountId = request.AccountId
        };

        await _dbContext.Set<StoredIncome>().AddAsync(newStoredIncome, cancellationToken);
    }

    public async Task DeleteLast(UserKey userId, CancellationToken cancellationToken)
    {
        var lastUserSpending = await _dbContext.Set<StoredIncome>()
            .Where(s => !s.IsDeleted && s.CreatedBy == userId)
            .OrderByDescending(s => s.CreatedDate)
            .FirstAsync(cancellationToken);

        lastUserSpending.IsDeleted = true;
    }
}