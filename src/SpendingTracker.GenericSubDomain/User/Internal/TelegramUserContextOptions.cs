namespace SpendingTracker.GenericSubDomain.User.Internal
{
    internal sealed class TelegramUserContextOptions
    {
        public TimeSpan CacheAbsoluteExpirationRelativeToNow { get; set; }
    }
}