using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Internal.Abstractions;
using SpendingTracker.TelegramBot.Internal.Buttons.ButtonContent;

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

        CurrenciesGroup = new ButtonGroup(_incrementalGroupId++, ButtonsGroupType.ChangeCurrency, "Выберите валюту");
        var settingsGroup = new ButtonGroup(_incrementalGroupId++, "Выберите настройку");
        settingsGroup
            .AddButtonsLayer(new Button("Валюта", CurrenciesGroup, settingsGroup))
            .AddButtonsLayer(new Button("Назад", StartGroup, settingsGroup));

        var createAnotherSpendingGroup = new RecursiveButtonGroup(_incrementalGroupId++, ButtonsGroupType.CreateAnotherSpending);
        var createSpendingGroup = new ButtonGroup(_incrementalGroupId++, ButtonsGroupType.CreateSpending, next: createAnotherSpendingGroup);

        createAnotherSpendingGroup
            .AddButtonsLayer(
                new Button(
                    "Удалить последнюю трату",
                    createSpendingGroup,
                    createAnotherSpendingGroup,
                    operation: ButtonOperation.DeleteLastSpending,
                    shouldEditPreviousMessage: false))
            .AddButtonsLayer(
                new Button("В меню", StartGroup, createAnotherSpendingGroup, false),
                new Button("Сменить валюту", CurrenciesGroup, createAnotherSpendingGroup));
        
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
            CurrenciesGroup
        };
    }

    public async Task<ButtonGroup> GetById(int id)
    {
        var targetButtonGroup = _groups.FirstOrDefault(g => g.Id == id);
        if (targetButtonGroup is null)
        {
            throw new ArgumentException($"Не найдена группа кнопок с идентификатором {id}");
        }

        if (targetButtonGroup.Type == ButtonsGroupType.ChangeCurrency)
        {
            targetButtonGroup.ClearButtons();

            var telegramUserId = _telegramUserIdStore.Id!.Value;
            var currencyButtons = await GetCurrencyButtonsForUser(telegramUserId);
            foreach (var currencyButton in currencyButtons)
            {
                targetButtonGroup.AddButtonsLayer(currencyButton);
            }

            targetButtonGroup.AddButtonsLayer(new Button("В меню", StartGroup, targetButtonGroup));
        }

        if (targetButtonGroup.Type == ButtonsGroupType.CreateAnotherSpending)
        {
            var telegramUserId = _telegramUserIdStore.Id!.Value;
            var user = await _userRepository.GetByTelegramId(telegramUserId);

            var text = string.Join(Environment.NewLine,
                $"Трата добавлена. Введите следующую, если необходимо",
                "-----------------------------------------------------------------------------------------",
                $"Текущая валюта: {user.Currency.CountryIcon}{user.Currency.Code}");
            targetButtonGroup.SetText(text);
        }

        return targetButtonGroup;
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
                    content: new CurrencyButtonContent(c.Code, c.CountryIcon)))
            .ToArray();

        return buttons;
    }
}