using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

public interface ICurrencyFactory
{
    Currency Create(StoredCurrency storedCurrency);
}