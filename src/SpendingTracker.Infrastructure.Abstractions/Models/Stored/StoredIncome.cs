using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored;

public class StoredIncome : EntityObject<StoredIncome, long>
{
    public long Id { get; set; }
    public double Amount { get; init; }
    public DateTimeOffset Date { get; init; }
    public string Description { get; init; }
    public Guid? AccountId { get; init; }
    public bool IsDeleted { get; set; }

    public override long GetKey()
    {
        return Id;
    }
}