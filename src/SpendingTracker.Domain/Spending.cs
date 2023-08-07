using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain;

public class Spending : EntityObject<Spending, Guid>
{
    public Guid Id { get; set; }

    public double Amount { get; set; }

    public Currency Currency { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Description { get; set; }

    public override Guid GetKey()
    {
        return Id;
    }
}