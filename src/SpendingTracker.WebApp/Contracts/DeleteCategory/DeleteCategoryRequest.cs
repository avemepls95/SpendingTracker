using SpendingTracker.Common.Primitives;

namespace SpendingTracker.WebApp.Contracts.DeleteCategory;

public class DeleteCategoryRequest
{
    public Guid Id { get; set; }
}