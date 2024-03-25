using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Internal.Buttons.ButtonContent;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;

namespace SpendingTracker.TelegramBot.Services.ButtonGroupTransformers;

public class ChangeCurrencyGroupTransformer : IButtonsGroupTransformer
{
    private readonly ITelegramUserIdStore _telegramUserIdStore;
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyRepository _currencyRepository;

    public ChangeCurrencyGroupTransformer(
        ITelegramUserIdStore telegramUserIdStore,
        IUserRepository userRepository,
        ICurrencyRepository currencyRepository)
    {
        _telegramUserIdStore = telegramUserIdStore;
        _userRepository = userRepository;
        _currencyRepository = currencyRepository;
    }

    public async Task Transform(ButtonGroup createIncomeGroup, int? returnGroupId, CancellationToken cancellationToken)
    {
        if (returnGroupId is null)
        {
            throw new Exception("Change currency group should have return button");
        }
        
        createIncomeGroup.ClearButtons();
        
        var returnGroup = ButtonsGroupStore.GetById(returnGroupId.Value);
        if (returnGroup is null)
        {
            throw new ArgumentException($"Не найдена группа кнопок с идентификатором {returnGroupId}");
        }

        var telegramUserId = _telegramUserIdStore.Id!.Value;
        var currencyButtons = await GetCurrencyButtonsForUser(telegramUserId, createIncomeGroup, returnGroup);
        foreach (var currencyButton in currencyButtons)
        {
            createIncomeGroup.AddButtonsLayer(currencyButton);
        }
        
        createIncomeGroup.AddButtonsLayer(new Button("Назад", returnGroup, createIncomeGroup));
    }

    private async Task<Button[]> GetCurrencyButtonsForUser(
        long telegramUserId,
        ButtonGroup currenciesGroup,
        ButtonGroup returnGroup)
    {
        var user = await _userRepository.GetByTelegramId(telegramUserId);
        var currencies = await _currencyRepository.GetAll();

        var currenciesExceptSelected = currencies.Except(new[] { user.Currency });
        var buttons = currenciesExceptSelected
            .OrderBy(c => c.Code)
            .Select(c => 
                new Button(
                    $"{c.CountryIcon}{c.Code} ({c.Title})",
                    returnGroup,
                    currenciesGroup,
                    shouldEditPreviousMessage: false,
                    content: new CurrencyButtonContent(c.Code, c.CountryIcon)))
            .ToArray();

        return buttons;
    }
}