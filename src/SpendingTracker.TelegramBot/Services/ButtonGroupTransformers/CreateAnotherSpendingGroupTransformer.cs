using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;

namespace SpendingTracker.TelegramBot.Services.ButtonGroupTransformers;

public class CreateAnotherSpendingGroupTransformer : IButtonsGroupTransformer
{
    private readonly ITelegramUserIdStore _telegramUserIdStore;
    private readonly IUserRepository _userRepository;

    public CreateAnotherSpendingGroupTransformer(
        ITelegramUserIdStore telegramUserIdStore,
        IUserRepository userRepository)
    {
        _telegramUserIdStore = telegramUserIdStore;
        _userRepository = userRepository;
    }

    public async Task Transform(ButtonGroup createAnotherSpendingGroup, int? returnGroupId)
    {
        var telegramUserId = _telegramUserIdStore.Id!.Value;
        var user = await _userRepository.GetByTelegramId(telegramUserId);

        var text = string.Join(Environment.NewLine,
            $"Трата добавлена. Введите следующую, если необходимо",
            "-----------------------------------------",
            $"Текущая валюта: {user.Currency.CountryIcon}{user.Currency.Code}");

        createAnotherSpendingGroup.SetText(text);
    }
}