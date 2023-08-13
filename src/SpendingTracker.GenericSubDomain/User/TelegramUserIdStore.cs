using SpendingTracker.GenericSubDomain.User.Abstractions;

namespace SpendingTracker.GenericSubDomain.User;

internal class TelegramUserIdStore : ITelegramUserIdStore
{
    public long? Id { get; set; }
}