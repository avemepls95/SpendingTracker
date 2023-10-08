using Microsoft.EntityFrameworkCore;
using SpendingTracker.Infrastructure.Abstractions.Models;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Infrastructure.Repositories;

public class CurrencyRateByDayRepository : ICurrencyRateByDayRepository
{
    private readonly MainDbContext _dbContext;

    private readonly Guid _baseCurrencyId = Guid.Parse("17d5494d-d969-465d-b5cc-16979e3fe5f8"); 

    public CurrencyRateByDayRepository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Create(DateTimeOffset date, Guid targetId, decimal coefficient, CancellationToken cancellationToken)
    {
        var newRecord = new StoredCurrencyRateByDay
        {
            Id = Guid.NewGuid(),
            Date = date,
            Base = _baseCurrencyId,
            Target = targetId,
            Coefficient = coefficient
        };

        await _dbContext.Set<StoredCurrencyRateByDay>().AddAsync(newRecord, cancellationToken);
    }

    public Task<bool> ExistsAnyInThisDay(DateTimeOffset day, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredCurrencyRateByDay>().AnyAsync(
            c => c.Date.Year == day.Year && c.Date.DayOfYear == day.DayOfYear,
            cancellationToken);
    }
}