using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Application.Handlers.Spending.AddExistToCategories.Contracts;

public class AddExistSpendingToNewCategoriesCommand
{
    public UserKey UserId { get; set; }
    public Guid SpendingId { get; set; }
    public string[] CategoryTitles { get; set; }
}