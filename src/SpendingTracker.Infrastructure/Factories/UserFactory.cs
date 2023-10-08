using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class UserFactory : IUserFactory
{
    private readonly ICurrencyFactory _currencyFactory;

    public UserFactory(ICurrencyFactory currencyFactory)
    {
        _currencyFactory = currencyFactory;
    }

    public User Create(StoredUser user)
    {
        var currency = _currencyFactory.Create(user.Currency);
        return new User(user.Id, currency);
    }
}