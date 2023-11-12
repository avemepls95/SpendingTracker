using Microsoft.Extensions.Caching.Memory;
using SpendingTracker.ApiClient;
using SpendingTracker.CurrencyRate.Abstractions;

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

    public async Task<Models.ExternalApiCurrencyRate[]> Get(string baseCode, string[] codes, CancellationToken cancellationToken)
    {
        var rates = await _memoryCache.GetOrCreateAsync("CurrencyRates", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
            
            var getCurrentRatesResult = await _currencyRateApiClient.GetByCodes(
                CurrencyOptions.BaseCurrencyCode,
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
    
        return rates.Select(c => new Models.ExternalApiCurrencyRate
        {
            SourceCode = c.SourceCode,
            TargetCode = c.TargetCode,
            Coefficient = c.Coefficient
        }).ToArray();
    }
}