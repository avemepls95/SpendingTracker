using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Internal.Abstractions;

namespace SpendingTracker.TelegramBot.Internal.Buttons;

internal class ButtonsGroupManager : IButtonsGroupManager
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly ITelegramUserIdStore _telegramUserIdStore;
    
    public ButtonGroup StartGroup { get; }
    private ButtonGroup CurrenciesGroup { get; }

    private readonly ButtonGroup[] _groups;

    private readonly int _incrementalGroupId = 0;

    public ButtonsGroupManager(
        IUserRepository userRepository,
        ICurrencyRepository currencyRepository,
        ITelegramUserIdStore telegramUserIdStore)
    {
        _userRepository = userRepository;
        _currencyRepository = currencyRepository;
        _telegramUserIdStore = telegramUserIdStore;

        StartGroup = new ButtonGroup(_incrementalGroupId++, "Выберите действие");

        CurrenciesGroup = new ButtonGroup(_incrementalGroupId++, ButtonsGroupOperation.ChangeCurrency, "Выберите валюту");
        var settingsGroup = new ButtonGroup(_incrementalGroupId++, "Выберите настройку");
        settingsGroup
            .AddButtonsLayer(new Button("Валюта", CurrenciesGroup, settingsGroup))
            .AddButtonsLayer(new Button("Назад", StartGroup, settingsGroup));
        
        var createAnotherSpendingGroup = new RecursiveButtonGroup(_incrementalGroupId++, ButtonsGroupOperation.CreateSpending, "Трата добавлена. Введите следующую, если необходимо");
        createAnotherSpendingGroup.AddButtonsLayer(new Button("В меню", StartGroup, createAnotherSpendingGroup, false));
        
        var createSpendingGroup = new ButtonGroup(_incrementalGroupId++, ButtonsGroupOperation.CreateSpending, next: createAnotherSpendingGroup);
        createSpendingGroup
            .AddButtonsLayer(new Button("Сменить валюту", CurrenciesGroup, createSpendingGroup))
            .AddButtonsLayer(new Button("Назад", StartGroup, createSpendingGroup));

        StartGroup
            .AddButtonsLayer(
                new Button("Перейти на сайт", "https://www.google.com"),
                new Button("Добавить трату ✏️", createSpendingGroup, StartGroup))
            .AddButtonsLayer(
                new Button("⚙️ Настройки", settingsGroup, StartGroup));

        _groups = new []
        {
            StartGroup,
            createSpendingGroup,
            createAnotherSpendingGroup,
            settingsGroup,
            CurrenciesGroup,
        };
    }

    public async Task<ButtonGroup> GetById(int id)
    {
        var result = _groups.FirstOrDefault(g => g.Id == id);
        if (result is null)
        {
            throw new ArgumentException($"Не найдена группа кнопок с идентификатором {id}");
        }

        if (result.Operation == ButtonsGroupOperation.ChangeCurrency)
        {
            result.ClearButtons();

            var telegramUserId = _telegramUserIdStore.Id!.Value;
            var currencyButtons = await GetCurrencyButtonsForUser(telegramUserId);
            foreach (var currencyButton in currencyButtons)
            {
                result.AddButtonsLayer(currencyButton);
            }

            result.AddButtonsLayer(new Button("В меню", StartGroup, result));
        }

        return result;
    }
    
    private async Task<Button[]> GetCurrencyButtonsForUser(long telegramUserId)
    {
        var user = await _userRepository.GetByTelegramId(telegramUserId);
        var currencies = await _currencyRepository.GetAll();

        var currenciesExceptSelected = currencies.Except(new[] { user.Currency });
        var buttons = currenciesExceptSelected
            .OrderBy(c => c.Code)
            .Select(c => 
                new Button(
                    $"{c.CountryIcon}{c.Code} ({c.Title})",
                    StartGroup,
                    CurrenciesGroup,
                    shouldEditPreviousMessage: false,
                    id: c.Code))
            .ToArray();

        return buttons;
    }
}