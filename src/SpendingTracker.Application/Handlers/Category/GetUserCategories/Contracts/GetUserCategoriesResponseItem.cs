namespace SpendingTracker.Application.Handlers.Category.GetUserCategories.Contracts;

public class GetUserCategoriesResponseItem
{
    public Guid Id { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public string Title { get; set; }
}