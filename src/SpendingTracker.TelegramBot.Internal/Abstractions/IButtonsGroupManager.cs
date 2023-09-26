using SpendingTracker.TelegramBot.Internal.Buttons;

namespace SpendingTracker.TelegramBot.Internal.Abstractions;

public interface IButtonsGroupManager
{
    public ButtonGroup StartGroup { get; }
    Task<ButtonGroup> ConstructById(int id, int? returnGroupId = null);
}