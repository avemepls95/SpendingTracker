using SpendingTracker.Common.Primitives;

namespace SpendingTracker.Domain;

public class TelegramUserCurrentOperation : EntityObject<TelegramUserCurrentOperation, Guid>
{
    public Guid Id { get; set; }
    public UserKey UserKey { get; set; }
    public TelegramUserCurrentOperationEnum Operation { get; set; }

    public override Guid GetKey()
    {
        return Id;
    }
}