namespace SpendingTracker.Common.Primitives
{
    public class UserKey : CustomKey<UserKey, Guid>
    {
        public UserKey(Guid value) : base(value)
        {
        }
    }
}