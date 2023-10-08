using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

internal interface ISpendingFactory
{
    Spending Create(StoredSpending storedSpending);
}