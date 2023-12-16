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

    private List<Category> _childs { get; set; } = new();
    /// <summary>
    /// Категории, которые включены в текущую.
    /// </summary>
    public Category[] Childs => _childs.ToArray();

    private List<Category> _parents { get; } = new();
    /// <summary>
    /// Категории, в которые включена текущая.
    /// </summary>
    public Category[] Parents => _parents.ToArray();

    public Category AddChild(Category child)
    {
        _childs.Add(child);

        return this;
    }
    
    public Category SetChilds(Category[] childs)
    {
        _childs = childs.ToList();

        return this;
    }
    
    public Category AddParent(Category child)
    {
        _parents.Add(child);

        return this;
    }
    
    public Category SetParents(Category[] parents)
    {
        _childs = parents.ToList();

        return this;
    }
    
    public override Guid GetKey()
    {
        return Id;
    }
}