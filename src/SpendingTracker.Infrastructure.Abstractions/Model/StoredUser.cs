using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Infrastructure.Abstractions.Model;

public class StoredUser : EntityObject<StoredUser, UserKey>
{
    public StoredUser(Guid id)
    {
        Id = new UserKey(id);
    }

    public UserKey Id { get; }

    public Currency Currency { get; set; }

    // TODO: Возможно, следует задать настройки системного юзера отдельно
    // public static StoredUser Default => new StoredUser(-1);
    public override UserKey GetKey()
    {
        return Id;
    }
}