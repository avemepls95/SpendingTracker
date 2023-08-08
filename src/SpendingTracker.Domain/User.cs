using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain
{
    public sealed class User
    {
        public User(long id)
        {
            Id = new UserKey(id);
        }

        public UserKey Id { get; }

        public Currency Currency { get; set; }

        // TODO: Возможно, следует задать настройки системного юзера отдельно
        public static User Default => new User(-1);
    }
}