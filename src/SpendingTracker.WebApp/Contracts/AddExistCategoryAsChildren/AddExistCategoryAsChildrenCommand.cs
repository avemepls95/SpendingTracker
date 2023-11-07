namespace SpendingTracker.WebApp.Contracts.AddExistCategoryAsChildren;

public class AddExistCategoryAsChildrenRequest
{
    public Guid ParentId { get; set; }
    public Guid ChildId { get; set; }
}