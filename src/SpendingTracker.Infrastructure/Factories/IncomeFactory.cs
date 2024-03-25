using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class IncomeFactory : IIncomeFactory
{
    public Income Create(StoredIncome storedIncome)
    {
        return new Income
        {
            Id = storedIncome.Id,
            Amount = storedIncome.Amount,
            Date = storedIncome.Date,
            Description = storedIncome.Description,
            AccountId = storedIncome.AccountId
        };
    }
}