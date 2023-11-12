using SpendingTracker.CurrencyRate.Contracts;

namespace SpendingTracker.CurrencyRate.Abstractions;

public interface ICurrencyConverter
{
    Task<GetCoefficientResult[]> GetCoefficients(
        GetCoefficientRequest[] requests,
        CancellationToken cancellationToken = default);
}