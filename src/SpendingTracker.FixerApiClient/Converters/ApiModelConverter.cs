using FixerApiClient.FixerApiModel.GetLastRates;
using Newtonsoft.Json.Linq;
using SpendingTracker.ApiClient;

namespace FixerApiClient.Converters;

// TODO: тесты
internal static class ApiModelConverter
{
    public static CurrencyRateFromApi[] ConvertGetLastRatesModels(GetLastRatesResponse apiModel)
    {
        if (apiModel.Rates is not JObject ratesFromResponse)
        {
            throw new ArgumentException(nameof(GetLastRatesResponse.Rates));
        }

        var result = new List<CurrencyRateFromApi>(ratesFromResponse.Count);
        
        foreach (var currencyProperty in ratesFromResponse)
        {
            var targetCode = currencyProperty.Key;
            var valueAsString = currencyProperty.Value?.ToString();
            
            if (!decimal.TryParse(valueAsString, out var value))
            {
                throw new Exception($"Не удалось распарсить значение валюты из FixerApi. Код {valueAsString}. Значение: {valueAsString}");
            }
            
            result.Add(new CurrencyRateFromApi
            {
                SourceCode = apiModel.BaseCode,
                TargetCode = targetCode,
                Coefficient = value
            });
        }

        return result.ToArray();
    }
}