using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class SpendingFactory : ISpendingFactory
{
    private readonly ICurrencyFactory _currencyFactory;

    public SpendingFactory(ICurrencyFactory currencyFactory)
    {
        _currencyFactory = currencyFactory;
    }

    public Spending Create(StoredSpending storedSpending, Guid[] categoryIds)
    {
        if (storedSpending.Currency is null)
        {
            throw new Exception("Валюта не может быть пустой");
        }

        var currency = _currencyFactory.Create(storedSpending.Currency);
        var spending = new Spending(
            storedSpending.Id,
            storedSpending.Amount,
            currency,
            storedSpending.Date,
            storedSpending.Description,
            storedSpending.ActionSource,
            categoryIds,
            storedSpending.CreatedDate);

        return spending;
    }
}