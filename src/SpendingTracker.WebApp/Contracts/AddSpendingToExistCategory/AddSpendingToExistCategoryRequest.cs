namespace SpendingTracker.WebApp.Contracts.AddSpendingToExistCategory;

public class AddSpendingToExistCategoryRequest
{
    public Guid SpendingId { get; set; }
    public Guid CategoryId { get; set; }
}