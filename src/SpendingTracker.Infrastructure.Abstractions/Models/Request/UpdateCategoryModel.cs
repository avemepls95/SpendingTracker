namespace SpendingTracker.Infrastructure.Abstractions.Models.Request;

public class UpdateCategoryModel
{
    public Guid Id { get; init; }
    public string Title { get; init; }
}