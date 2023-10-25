namespace SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public CategoryDto[] Parents { get; set; }
}