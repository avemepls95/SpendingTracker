namespace SpendingTracker.TelegramBot;

public class ButtonsGroupManager
{
    public ButtonGroup StartButtonsGroup { get; }
    private readonly ButtonGroup[] _groups;

    private ButtonsGroupManager()
    {
        StartButtonsGroup = new ButtonGroup("Выберите действие");
        var level2ButtonsGroup = new ButtonGroup(
            "Введите трату в формате сумма/дата/описание (каждое значение на новой строке)",
            UserOperationEnum.CreateSpending);

        StartButtonsGroup.AddButtonsLayer(
            new Button("Добавить трату", level2ButtonsGroup, StartButtonsGroup),
            new Button("Перейти на сайт", "https://www.google.com", StartButtonsGroup));

        level2ButtonsGroup.AddButtonsLayer(new Button("Назад", StartButtonsGroup, level2ButtonsGroup));
        _groups = new []
        {
            StartButtonsGroup,
            level2ButtonsGroup,
        };
    }

    public static ButtonsGroupManager Initialize()
    {
        return new ButtonsGroupManager();
    }

    public ButtonGroup GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        var result = _groups.FirstOrDefault(g => g.Id.ToString() == id);
        if (result is null)
        {
            throw new ArgumentException($"Не найдена группа кнопок с идентификатором {id}");

        }

        return result;
    }
}