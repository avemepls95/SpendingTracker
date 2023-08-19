using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Categories;

namespace SpendingTracker.Domain;

public class Spending : EntityObject<Spending, Guid>
{
    public Spending(
        Guid id,
        double amount,
        Currency currency,
        DateTimeOffset date,
        string description,
        ActionSource actionSource)
    {
        Id = id;
        Amount = amount;
        Currency = currency;
        Date = date;
        Description = description;
        ActionSource = actionSource;
    }
    
    public Guid Id { get; init; }

    public double Amount { get; init; }

    public Currency Currency { get; init; }

    public DateTimeOffset Date { get; init; }

    public string Description { get; init; }

    public ActionSource ActionSource { get; init; }

    /// <summary>
    /// Категории, в которые включена трата.
    /// </summary>
    public Category[] ParentCategories { get; private set; }

    public override Guid GetKey()
    {
        return Id;
    }

    public Spending SetParentCategories(Category[] categories)
    {
        ParentCategories = categories;

        return this;
    }
}