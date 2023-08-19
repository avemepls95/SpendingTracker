using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain.Categories;

public class Category : EntityObject<Category, Guid>
{
    public Category(Guid id, UserKey ownerId, string title)
    {
        Id = id;
        OwnerId = ownerId;
        Title = title;
    }
    
    public Guid Id { get; }
    public UserKey OwnerId { get; }
    public string Title { get; }

    /// <summary>
    /// Категории, которые включены в текущую.
    /// </summary>
    public Category[] ChildCategories { get; private set; } = Array.Empty<Category>();

    /// <summary>
    /// Категории, в которые включена текущая.
    /// </summary>
    public Category[] ParentCategories { get; set; } = Array.Empty<Category>();

    public Category SetChildCategories(Category[] categories)
    {
        ChildCategories = categories;

        return this;
    }
    
    public Category SetParentCategories(Category[] categories)
    {
        ParentCategories = categories;

        return this;
    }

    public override Guid GetKey()
    {
        return Id;
    }
}