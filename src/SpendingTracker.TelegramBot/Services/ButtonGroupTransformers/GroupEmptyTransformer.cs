using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;

namespace SpendingTracker.TelegramBot.Services.ButtonGroupTransformers;

public class GroupEmptyTransformer : IButtonsGroupTransformer
{
    public Task Transform(ButtonGroup group, int? returnGroupId)
    {
        return Task.CompletedTask;
    }
}