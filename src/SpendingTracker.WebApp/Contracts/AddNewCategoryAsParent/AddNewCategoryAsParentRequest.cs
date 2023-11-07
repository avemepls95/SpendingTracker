namespace SpendingTracker.WebApp.Contracts.AddNewCategoryAsParent;

public class AddNewCategoryAsParentRequest
{
    public Guid ChildId { get; set; }
    public string NewParentTitle { get; set; }
}