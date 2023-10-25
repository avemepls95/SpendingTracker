using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored.Categories;

public class StoredCategory : EntityObject<StoredCategory, Guid>
{
    public Guid Id { get; set; }
    public UserKey OwnerId { get; set; }
    public string Title { get; set; }   
    public bool IsDeleted { get; set; }

    public override Guid GetKey()
    {
        return Id;
    }
}