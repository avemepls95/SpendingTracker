namespace SpendingTracker.WebApp.Contracts.UpdateCategory;

public class UpdateCategoryRequest
{
    public Guid Id { get; init; }
    public string Title { get; init; }
}