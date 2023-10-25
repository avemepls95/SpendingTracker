using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain;

public class Spending : EntityObject<Spending, Guid>
{
    public Spending(
        Guid id,
        double amount,
        Currency currency,
        DateTimeOffset date,
        string description,
        ActionSource actionSource,
        Guid[] categoryIds,
        DateTimeOffset? createDate = null)
    {
        Id = id;
        Amount = amount;
        Currency = currency;
        Date = date;
        Description = description;
        ActionSource = actionSource;
        CategoryIds = categoryIds;

        if (createDate is not null)
        {
            CreatedDate = createDate.Value;
        }
    }
    
    public Guid Id { get; }

    public double Amount { get; }

    public Currency Currency { get; }

    public DateTimeOffset Date { get; }

    public string Description { get; }

    public ActionSource ActionSource { get; }

    public Guid[] CategoryIds { get; }

    public override Guid GetKey()
    {
        return Id;
    }
}