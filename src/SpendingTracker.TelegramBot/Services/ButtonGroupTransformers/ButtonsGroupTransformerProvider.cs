using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;

namespace SpendingTracker.TelegramBot.Services.ButtonGroupTransformers;

public class ButtonsGroupTransformerProvider : IButtonsGroupTransformerProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ButtonsGroupTransformerProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IButtonsGroupTransformer Get(int groupId)
    {
        var group = ButtonsGroupStore.GetById(groupId);
        return group.Type switch
        {
            ButtonsGroupType.CreateAnotherSpending => _serviceProvider.GetService<CreateAnotherSpendingGroupTransformer>()!,
            ButtonsGroupType.ChangeCurrency => _serviceProvider.GetService<ChangeCurrencyGroupTransformer>()!,
            ButtonsGroupType.None => new GroupEmptyTransformer(),
            ButtonsGroupType.CreateSpending => new GroupEmptyTransformer(),
            ButtonsGroupType.CreateIncome => new GroupEmptyTransformer(),
            _ => throw new ArgumentOutOfRangeException(nameof(ButtonsGroupType), group.Type, null)
        };
    }
}