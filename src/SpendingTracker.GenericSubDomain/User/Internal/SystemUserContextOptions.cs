namespace SpendingTracker.GenericSubDomain.User.Internal
{
    public sealed class SystemUserContextOptions
    {
        public TimeSpan CacheAbsoluteExpirationRelativeToNow { get; set; }
    }
}