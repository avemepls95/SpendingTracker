using Microsoft.EntityFrameworkCore;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

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

    public Task<CurrencyRateByDayShortModel[]> GetRatesByDays(DateOnly[] dates, CancellationToken cancellationToken)
    {
        return _dbContext.Set<StoredCurrencyRateByDay>()
            .Where(r => dates.Any(d => DateOnly.FromDateTime(r.Date.Date) == d))
            .Select(r => new CurrencyRateByDayShortModel
            {
                Date = r.Date,
                Base = r.Base,
                Coefficient = r.Coefficient,
                Target = r.Target
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<CurrencyRateByDayShortModel[]> GetRatesByNearestDay(DateOnly date, CancellationToken cancellationToken)
    {
        var sqlRaw = $@"
SELECT *
FROM ""CurrencyRateByDay"" AS c
ORDER BY ABS(CAST((date_trunc('day', c.""Date"" AT TIME ZONE 'UTC') AT TIME ZONE 'UTC') AS date) - '{date.ToString()}')
LIMIT 1
";

        var nearestDate = await _dbContext
            .Set<StoredCurrencyRateByDay>().FromSqlRaw(sqlRaw)
            .Select(r => r.Date)
            .FirstOrDefaultAsync(cancellationToken);

        var result = await _dbContext.Set<StoredCurrencyRateByDay>()
            .Where(r => r.Date.Day == nearestDate.Day)
            .Select(r => new CurrencyRateByDayShortModel
            {
                Date = r.Date,
                Base = r.Base,
                Coefficient = r.Coefficient,
                Target = r.Target
            })
            .ToArrayAsync(cancellationToken);
        
        return result;
    }
}