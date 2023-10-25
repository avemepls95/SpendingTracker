namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;

public class StoredSpendingCategoryLink
{
    public Guid SpendingId { get; set; }

    public StoredSpending Spending { get; set; }

    public Guid CategoryId { get; set; }

    public StoredCategory Category { get; set; }
}