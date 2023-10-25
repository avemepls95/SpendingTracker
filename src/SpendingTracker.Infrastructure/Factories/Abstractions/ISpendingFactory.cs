using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

internal interface ISpendingFactory
{
    Spending Create(StoredSpending storedSpending, Guid[] categoryIds);
}