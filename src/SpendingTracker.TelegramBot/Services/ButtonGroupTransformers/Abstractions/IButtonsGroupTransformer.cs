using SpendingTracker.TelegramBot.Internal.Buttons;

namespace SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;

public interface IButtonsGroupTransformer
{
    Task Transform(ButtonGroup createIncomeGroup, int? returnGroupId, CancellationToken cancellationToken = default);
}