using SpendingTracker.CurrencyRate.Abstractions;

namespace SpendingTracker.CurrencyRate;

// TODO: тесты
internal class CurrencyConverter : ICurrencyConverter
{
    public async Task<decimal> GetCoefficient(string code, CancellationToken cancellationToken)
    {
        if (CurrencyOptions.BaseCurrencyCode == code)
        {
            return 1;
        }

        return 1;
    }
}