using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored;

public class StoredCurrencyRateByDay: EntityObject<StoredCurrencyRateByDay, Guid>
{
    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public Guid Base { get; set; }
    public Guid Target { get; set; }
    public decimal Coefficient { get; set; }
    public override Guid GetKey()
    {
        return Id;
    }
}