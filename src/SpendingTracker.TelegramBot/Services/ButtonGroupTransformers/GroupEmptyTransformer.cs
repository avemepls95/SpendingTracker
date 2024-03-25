using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;

namespace SpendingTracker.TelegramBot.Services.ButtonGroupTransformers;

public class GroupEmptyTransformer : IButtonsGroupTransformer
{
    public Task Transform(ButtonGroup createIncomeGroup, int? returnGroupId, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}