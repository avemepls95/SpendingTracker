using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Abstractions.Model.Categories;

public class StoredCategory : EntityObject<StoredCategory, Guid>
{
    public Guid Id { get; set; }
    public UserKey OwnerId { get; set; }
    public string Title { get; set; }
    public List<CategoriesLink> ChildCategoryLinks { get; set; } = new List<CategoriesLink>();
    public List<CategoriesLink> ParentCategoryLinks { get; set; } = new List<CategoriesLink>();

    public override Guid GetKey()
    {
        return Id;
    }
}