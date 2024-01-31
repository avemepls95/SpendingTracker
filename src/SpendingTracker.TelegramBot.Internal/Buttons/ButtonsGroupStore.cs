namespace SpendingTracker.TelegramBot.Internal.Buttons;

public static class ButtonsGroupStore
{
    public static ButtonGroup StartGroup { get; }
    public static ButtonGroup CreateAnotherSpendingGroup { get; }
    private static ButtonGroup CurrenciesGroup { get; }

    private static readonly ButtonGroup[] _groups;

    private static readonly int _incrementalGroupId = 0;

    static ButtonsGroupStore()
    {
        StartGroup = new ButtonGroup(_incrementalGroupId++, "Выберите действие");

        CurrenciesGroup = new ButtonGroup(_incrementalGroupId++, ButtonsGroupType.ChangeCurrency, "Выберите валюту");
        var settingsGroup = new ButtonGroup(_incrementalGroupId++, "Выберите настройку");
        settingsGroup
            .AddButtonsLayer(new Button("Валюта", CurrenciesGroup, settingsGroup))
            .AddButtonsLayer(new Button("В меню", StartGroup, settingsGroup));

        CreateAnotherSpendingGroup = new RecursiveButtonGroup(_incrementalGroupId++, ButtonsGroupType.CreateAnotherSpending);
        var createSpendingGroup = new ButtonGroup(_incrementalGroupId++, ButtonsGroupType.CreateSpending);
        var createIncomeGroup = new ButtonGroup(_incrementalGroupId++, ButtonsGroupType.CreateIncome);

        CreateAnotherSpendingGroup
            .AddButtonsLayer(
                new Button(
                    "Удалить последнюю трату",
                    createSpendingGroup,
                    CreateAnotherSpendingGroup,
                    operation: ButtonOperation.DeleteLastSpending,
                    shouldEditPreviousMessage: false))
            .AddButtonsLayer(
                new Button("В меню", StartGroup, CreateAnotherSpendingGroup, false),
                new Button("Сменить валюту", CurrenciesGroup, CreateAnotherSpendingGroup));
        
        createSpendingGroup
            .AddButtonsLayer(new Button("Сменить валюту", CurrenciesGroup, createSpendingGroup))
            .AddButtonsLayer(new Button("Назад", StartGroup, createSpendingGroup));

        StartGroup
            .AddButtonsLayer(new Button("✏️ Добавить трату", createSpendingGroup, StartGroup))
            .AddButtonsLayer(new Button("💵 Добавить доход", createIncomeGroup, StartGroup))
            .AddButtonsLayer(new Button("⚙️ Настройки", settingsGroup, StartGroup));

        _groups = new []
        {
            StartGroup,
            createSpendingGroup,
            CreateAnotherSpendingGroup,
            settingsGroup,
            CurrenciesGroup
        };
    }

    public static ButtonGroup GetById(int id)
    {
        var targetButtonGroup = _groups.FirstOrDefault(g => g.Id == id);
        if (targetButtonGroup is null)
        {
            throw new ArgumentException($"Не найдена группа кнопок с идентификатором {id}");
        }

        // if (targetButtonGroup.Type == ButtonsGroupType.ChangeCurrency)
        // {
        //     targetButtonGroup.ClearButtons();
        //
        //     var returnGroup = StartGroup;
        //
        //     if (returnGroupId is not null)
        //     {
        //         returnGroup = _groups.FirstOrDefault(g => g.Id == returnGroupId);
        //         if (returnGroup is null)
        //         {
        //             throw new ArgumentException($"Не найдена группа кнопок с идентификатором {id}");
        //         }
        //     }
        //     
        //     var telegramUserId = _telegramUserIdStore.Id!.Value;
        //     var currencyButtons = await GetCurrencyButtonsForUser(telegramUserId, returnGroup);
        //     foreach (var currencyButton in currencyButtons)
        //     {
        //         targetButtonGroup.AddButtonsLayer(currencyButton);
        //     }
        //     
        //     targetButtonGroup.AddButtonsLayer(new Button("Назад", returnGroup, targetButtonGroup));
        // }

        // if (targetButtonGroup.Type == ButtonsGroupType.CreateAnotherSpending)
        // {
        //     var telegramUserId = _telegramUserIdStore.Id!.Value;
        //     var user = await _userRepository.GetByTelegramId(telegramUserId);
        //
        //     var text = string.Join(Environment.NewLine,
        //         $"Трата добавлена. Введите следующую, если необходимо",
        //         "-----------------------------------------",
        //         $"Текущая валюта: {user.Currency.CountryIcon}{user.Currency.Code}");
        //     targetButtonGroup.SetText(text);
        // }

        return targetButtonGroup;
    }
    
    // private async Task<Button[]> GetCurrencyButtonsForUser(long telegramUserId, ButtonGroup returnGroup)
    // {
    //     var user = await _userRepository.GetByTelegramId(telegramUserId);
    //     var currencies = await _currencyRepository.GetAll();
    //
    //     var currenciesExceptSelected = currencies.Except(new[] { user.Currency });
    //     var buttons = currenciesExceptSelected
    //         .OrderBy(c => c.Code)
    //         .Select(c => 
    //             new Button(
    //                 $"{c.CountryIcon}{c.Code} ({c.Title})",
    //                 returnGroup,
    //                 CurrenciesGroup,
    //                 shouldEditPreviousMessage: false,
    //                 content: new CurrencyButtonContent(c.Code, c.CountryIcon)))
    //         .ToArray();
    //
    //     return buttons;
    // }
}