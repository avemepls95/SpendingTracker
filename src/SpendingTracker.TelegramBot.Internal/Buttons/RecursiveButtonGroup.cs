namespace SpendingTracker.TelegramBot.Internal.Buttons;

public class RecursiveButtonGroup : ButtonGroup
{
    public RecursiveButtonGroup(int id, ButtonsGroupType type, string? text = null)
        : base(id, type, text)
    {
        // Next = this;
    }

}