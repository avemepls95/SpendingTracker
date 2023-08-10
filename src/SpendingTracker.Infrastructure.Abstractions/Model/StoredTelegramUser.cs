using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Infrastructure.Abstractions.Model;

public class StoredTelegramUser : EntityObject<StoredTelegramUser, long>
{
    public long Id { get; set; }
    public UserKey UserId { get; set; }

    public override long GetKey()
    {
        return Id;
    }
}