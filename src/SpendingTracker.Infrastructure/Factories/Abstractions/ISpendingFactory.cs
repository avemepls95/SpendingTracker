using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

public interface ISpendingFactory
{
    Spending Create(StoredSpending storedSpending);
}