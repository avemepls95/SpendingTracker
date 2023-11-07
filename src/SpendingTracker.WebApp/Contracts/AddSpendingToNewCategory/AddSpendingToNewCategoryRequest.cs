namespace SpendingTracker.WebApp.Contracts.AddSpendingToNewCategory;

public class AddSpendingToNewCategoryRequest
{
    public Guid SpendingId { get; set; }
    public string NewCategoryTitle { get; set; }
}