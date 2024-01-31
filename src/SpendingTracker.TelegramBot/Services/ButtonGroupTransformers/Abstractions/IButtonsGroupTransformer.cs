using SpendingTracker.TelegramBot.Internal.Buttons;

namespace SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;

public interface IButtonsGroupTransformer
{
    Task Transform(ButtonGroup group, int? returnGroupId);
}