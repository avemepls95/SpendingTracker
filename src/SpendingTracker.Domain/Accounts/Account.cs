using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain.Accounts;

public class Account : EntityObject<Account, Guid>
{
    public Guid Id { get; set; }
    public UserKey UserId { get; set; }
    public AccountTypeEnum Type { get; set; }
    public string Name { get; set; }
    public Guid CurrencyId { get; set; }
    public double Amount { get; set; }
    public bool IsDeleted { get; set; }
    
    public override Guid GetKey()
    {
        return Id;
    }
}