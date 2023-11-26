namespace SpendingTracker.GenericSubDomain.User.Internal
{
    public sealed class TelegramUserContextOptions
    {
        public TimeSpan CacheAbsoluteExpirationRelativeToNow { get; set; }
    }
}