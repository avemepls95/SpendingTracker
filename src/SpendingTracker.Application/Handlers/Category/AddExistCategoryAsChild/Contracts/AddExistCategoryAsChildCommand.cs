namespace SpendingTracker.Application.Handlers.Category.AddExistCategoryAsChild.Contracts;

public class AddExistCategoryAsChildCommand
{
    public Guid ParentId { get; set; }
    public Guid ChildId { get; set; }
}