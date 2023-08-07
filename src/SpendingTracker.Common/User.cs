using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Common
{
    public sealed class User
    {
        public User(long id)
        {
            Id = new UserKey(id);
        }

        public UserKey Id { get; }

        // TODO: Возможно, следует задать настройки системного юзера отдельно
        public static User Default => new User(-1);
    }
}