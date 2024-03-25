using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain;

public class Income : EntityObject<Income, long>
{
    public long Id { get; init; }
    public double Amount { get; init; }
    public DateTimeOffset Date { get; init; }
    public string Description { get; init; }
    public Guid? AccountId { get; init; }
    
    public override long GetKey()
    {
        return Id;
    }
}