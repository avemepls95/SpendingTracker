using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Accounts;

namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored;

public class StoredAccount : EntityObject<StoredAccount, Guid>
{
    public Guid Id { get; set; }
    public UserKey UserId { get; set; }
    public AccountTypeEnum Type { get; set; }
    public string Name { get; set; }
    public Guid CurrencyId { get; set; }
    public StoredCurrency Currency { get; set; }
    public double Amount { get; set; }
    public bool IsDeleted { get; set; }
    
    public override Guid GetKey()
    {
        return Id;
    }
}