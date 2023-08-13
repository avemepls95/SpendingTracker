namespace SpendingTracker.GenericSubDomain.User.Abstractions;

public interface ITelegramUserIdStore
{
    public long? Id { get; set; }
}