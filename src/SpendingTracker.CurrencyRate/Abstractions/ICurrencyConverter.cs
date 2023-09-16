namespace SpendingTracker.CurrencyRate.Abstractions;

public interface ICurrencyConverter
{
    Task<decimal> GetCoefficient(string code, CancellationToken cancellationToken = default);
}