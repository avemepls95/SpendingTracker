using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Abstractions.Model;

public class StoredUser : EntityObject<StoredUser, UserKey>
{
    public StoredUser(UserKey id, Guid currencyId)
    {
        Id = id;
        CurrencyId = currencyId;
    }

    public UserKey Id { get; }
    public Guid CurrencyId { get; set; }
    public StoredCurrency Currency { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }

    public override UserKey GetKey()
    {
        return Id;
    }
}