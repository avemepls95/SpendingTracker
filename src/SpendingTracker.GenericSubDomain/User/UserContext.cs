using SpendingTracker.Common.Primitives;

namespace SpendingTracker.GenericSubDomain.User
{
    public class UserContext
    {
        public UserKey UserId { get; }

        public UserContext(UserKey userId)
        {
            UserId = userId;
        }
    }
}