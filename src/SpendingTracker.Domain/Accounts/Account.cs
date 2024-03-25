using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain.Accounts;

public class Account : EntityObject<Account, Guid>
{
    public Account(
        Guid id,
        UserKey userId,
        AccountTypeEnum type,
        string name,
        Guid currencyId,
        double amount,
        DateTimeOffset createDate)
    {
        Id = id;
        UserId = userId;
        Type = type;
        Name = name;
        CurrencyId = currencyId;
        Amount = amount;
        CreatedDate = createDate;
    }
    
    public Guid Id { get; private set; }
    public UserKey UserId { get; private set; }
    public AccountTypeEnum Type { get; private set; }
    public string Name { get; private set; }
    public Guid CurrencyId { get; private set; }
    public double Amount { get; private set; }
    public bool IsDeleted { get; private set; }
    
    public override Guid GetKey()
    {
        return Id;
    }
}