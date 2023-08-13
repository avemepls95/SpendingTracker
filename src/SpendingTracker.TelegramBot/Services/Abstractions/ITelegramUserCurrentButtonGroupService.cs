using SpendingTracker.TelegramBot.Buttons;

namespace SpendingTracker.TelegramBot.Services.Abstractions;

public interface ITelegramUserCurrentButtonGroupService
{
    Task<ButtonGroup> GetGroupByUserId(long id, CancellationToken cancellationToken = default);

    Task Update(long userId, ButtonGroup newGroup, CancellationToken cancellationToken = default);
}