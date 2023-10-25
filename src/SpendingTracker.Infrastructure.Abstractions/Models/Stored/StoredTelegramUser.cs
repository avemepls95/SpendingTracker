using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Abstractions.Models.Stored;

public class StoredTelegramUser : EntityObject<StoredTelegramUser, long>
{
    public long Id { get; set; }
    public UserKey UserId { get; set; }
    public StoredUser User { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public bool IsDeleted { get; set; }

    public override long GetKey()
    {
        return Id;
    }
}