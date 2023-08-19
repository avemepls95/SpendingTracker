namespace SpendingTracker.TelegramBot.Buttons;

public class ButtonsGroupManager
{
    public ButtonGroup StartButtonsGroup { get; }
    private readonly ButtonGroup[] _groups;
    private static readonly ButtonsGroupManager Instance = new ButtonsGroupManager();

    private readonly int _incrementalGroupId = 0;

    private ButtonsGroupManager()
    {
        StartButtonsGroup = new ButtonGroup(_incrementalGroupId++, "Выберите действие");

        var createAnotherSpendingGroup = new RecursiveButtonGroup(_incrementalGroupId++, ButtonsGroupOperation.CreateSpending, "Трата добавлена. Введите следующую, если необходимо");
        createAnotherSpendingGroup.AddButtonsLayer(new Button("В меню", StartButtonsGroup, createAnotherSpendingGroup, false));
        
        var createSpendingGroup = new ButtonGroup(_incrementalGroupId++, ButtonsGroupOperation.CreateSpending, next: createAnotherSpendingGroup);
        createSpendingGroup.AddButtonsLayer(new Button("Назад", StartButtonsGroup, createSpendingGroup));

        var settingsGroup = new ButtonGroup(_incrementalGroupId++, "Здесь скоро будут настройки😊");
        settingsGroup.AddButtonsLayer(new Button("Назад", StartButtonsGroup, settingsGroup));
        
        StartButtonsGroup
            .AddButtonsLayer(
                new Button("Перейти на сайт", "https://www.google.com", StartButtonsGroup),
                new Button("Добавить трату ✏️", createSpendingGroup, StartButtonsGroup))
            .AddButtonsLayer(
                new Button("⚙️ Настройки", settingsGroup, StartButtonsGroup));

        _groups = new []
        {
            StartButtonsGroup,
            createSpendingGroup,
            createAnotherSpendingGroup,
            settingsGroup
        };
    }

    public static ButtonsGroupManager GetInstance()
    {
        return Instance;
    }

    public ButtonGroup GetById(int id)
    {
        var result = _groups.FirstOrDefault(g => g.Id == id);
        if (result is null)
        {
            throw new ArgumentException($"Не найдена группа кнопок с идентификатором {id}");
        }

        return result;
    }
}