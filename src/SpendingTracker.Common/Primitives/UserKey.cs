namespace SpendingTracker.Common.Primitives
{
    public class UserKey : CustomKey<UserKey, long>
    {
        public UserKey(long value) : base(value)
        {
        }
    }
}