using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

internal interface ICurrencyFactory
{
    Currency Create(StoredCurrency storedCurrency);
}