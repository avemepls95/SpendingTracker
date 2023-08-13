namespace SpendingTracker.TelegramBot.Buttons;

public class ButtonsGroupManager
{
    public ButtonGroup Level1ButtonsGroup { get; }
    private readonly ButtonGroup[] _groups;
    private static readonly ButtonsGroupManager Instance = new();

    private ButtonsGroupManager()
    {
        Level1ButtonsGroup = new ButtonGroup(1,"Выберите действие");
        var goToLevel1Button = new Button("В меню", Level1ButtonsGroup);
        var level2ButtonsGroup = new ButtonGroup(
            2,
            ButtonsGroupOperation.CreateSpending,
            next: new RecursiveButtonGroup(
                    3,
                    ButtonsGroupOperation.CreateSpending,
                    "Трата добавлена. Введите следующую, если необходимо")
                .AddButtonsLayer(goToLevel1Button));
        
        Level1ButtonsGroup.AddButtonsLayer(
            new Button("Добавить трату", level2ButtonsGroup),
            new Button("Перейти на сайт", "https://www.google.com"));

        level2ButtonsGroup.AddButtonsLayer(new Button("Назад", Level1ButtonsGroup));
        _groups = new []
        {
            Level1ButtonsGroup,
            level2ButtonsGroup,
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