namespace SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

public class SpendingCategoryLink
{
    public Guid SpendingId { get; set; }
    public Guid CategoryId { get; set; }
}