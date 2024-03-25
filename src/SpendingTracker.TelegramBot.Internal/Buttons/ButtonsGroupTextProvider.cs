namespace SpendingTracker.TelegramBot.Internal.Buttons;

public class ButtonsGroupTextProvider
{
    private static readonly Dictionary<ButtonsGroupType, string> OperationTextDict = new()
    {
        { 
            ButtonsGroupType.CreateSpending,
@"Введите трату в следующем формате:

Cумма
Описание
Дата (опционально)"
        },
        { ButtonsGroupType.CreateAnotherSpending, "Трата добавлена. Введите следующую, если необходимо" },
    };

    public static string? FindText(ButtonsGroupType type)
    {
        return OperationTextDict.TryGetValue(type, out var result)
            ? result
            : null;
    }
}