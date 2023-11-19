using Microsoft.Extensions.Caching.Memory;
using SpendingTracker.ApiClient;
using SpendingTracker.CurrencyRate.Abstractions;
using SpendingTracker.CurrencyRate.Models;

namespace SpendingTracker.CurrencyRate;

internal sealed class RatesProvider : IRatesProvider
{
    private readonly ICurrencyRateApiClient _currencyRateApiClient;
    private readonly IMemoryCache _memoryCache;

    public RatesProvider(ICurrencyRateApiClient currencyRateApiClient, IMemoryCache memoryCache)
    {
        _currencyRateApiClient = currencyRateApiClient;
        _memoryCache = memoryCache;
    }

    public async Task<ExternalApiTodayCurrencyRate[]> GetToday(string baseCode, string[] codes, CancellationToken cancellationToken)
    {
        var rates = await _memoryCache.GetOrCreateAsync("CurrencyRates", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
            
            var getCurrentRatesResult = await _currencyRateApiClient.GetToday(
                baseCode,
                codes,
                cancellationToken);
            
            if (!getCurrentRatesResult.IsSuccess)
            {
                throw new Exception(getCurrentRatesResult.ErrorMessage);
            }
    
            return getCurrentRatesResult.Result;
        });
    
        if (rates is null)
        {
            throw new Exception("Не удалось получить данные о курсах валют.");
        }
    
        return rates.Select(c => new ExternalApiTodayCurrencyRate
        {
            SourceCode = c.SourceCode,
            TargetCode = c.TargetCode,
            Coefficient = c.Coefficient
        }).ToArray();
    }

    public async Task<ExternalApiCurrencyByDayRate[]> GetByDates(
        string baseCode,
        string[] codes,
        DateOnly[] dates,
        CancellationToken cancellationToken)
    {
        var getRatesByDaysResult = await _currencyRateApiClient.GetByDates(
            baseCode,
            codes,
            dates,
            cancellationToken);
            
        if (!getRatesByDaysResult.IsSuccess)
        {
            throw new Exception(getRatesByDaysResult.ErrorMessage);
        }
        
        return getRatesByDaysResult.Result.Select(c => new ExternalApiCurrencyByDayRate
        {
            SourceCode = c.SourceCode,
            TargetCode = c.TargetCode,
            Coefficient = c.Coefficient,
            Date = c.Date
        }).ToArray();
    }
}