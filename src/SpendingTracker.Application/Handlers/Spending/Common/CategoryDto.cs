namespace SpendingTracker.Application.Handlers.Spending.Common;

public class CategoryDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public CategoryDto[] Parents { get; set; }
}