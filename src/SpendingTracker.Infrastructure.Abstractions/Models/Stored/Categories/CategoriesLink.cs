namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;

public class CategoriesLink
{
    public Guid ChildId { get; set; }
    public StoredCategory Child { get; set; }
    public Guid ParentId { get; set; }
    public StoredCategory Parent { get; set; }
}