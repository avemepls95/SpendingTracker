namespace SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;

public interface IButtonsGroupTransformerProvider
{
    IButtonsGroupTransformer Get(int groupId);
}