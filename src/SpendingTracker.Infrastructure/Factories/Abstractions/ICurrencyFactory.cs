using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

internal interface ICurrencyFactory
{
    Currency Create(StoredCurrency storedCurrency);
}