using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class SpendingFactory : ISpendingFactory
{
    private readonly ICurrencyFactory _currencyFactory;
    private readonly ICategoryFactory _categoryFactory;

    public SpendingFactory(ICurrencyFactory currencyFactory, ICategoryFactory categoryFactory)
    {
        _currencyFactory = currencyFactory;
        _categoryFactory = categoryFactory;
    }

    public Spending Create(StoredSpending storedSpending)
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
            storedSpending.ActionSource);

        if (storedSpending.CategoryLinks != null)
        {
            var categories = storedSpending.CategoryLinks.Select(l => _categoryFactory.Create(l.Category)).ToArray();
            spending.SetParentCategories(categories);
        }
        
        return spending;
    }
}