namespace SpendingTracker.GenericSubDomain.User.Internal
{
    internal sealed class SystemUserContextOptions
    {
        public TimeSpan CacheAbsoluteExpirationRelativeToNow { get; set; }
    }
}