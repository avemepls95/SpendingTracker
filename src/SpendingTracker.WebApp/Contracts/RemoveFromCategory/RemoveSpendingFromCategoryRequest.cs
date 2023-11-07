namespace SpendingTracker.WebApp.Contracts.RemoveFromCategory;

public class RemoveSpendingFromCategoryRequest
{
    public Guid SpendingId { get; set; }
    public Guid CategoryId { get; set; }
}