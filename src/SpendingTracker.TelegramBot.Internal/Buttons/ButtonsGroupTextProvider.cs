namespace SpendingTracker.TelegramBot.Internal.Buttons;

public class ButtonsGroupTextProvider
{
    private static readonly Dictionary<ButtonsGroupType, string> OperationTextDict = new()
    {
        { 
            ButtonsGroupType.CreateSpending,
@"Введите трату в формате

Cумма
Описание
Дата (опционально)"
        },
        { ButtonsGroupType.CreateAnotherSpending, "Трата добавлена. Введите следующую, если необходимо" },
        {
            ButtonsGroupType.CreateIncome,
            @"Введите доход в формате

Cумма
Описание
Номер счета списания
Дата (опционально)"
        },
    };

    public static string GetText(ButtonsGroupType type)
    {
        if (OperationTextDict.TryGetValue(type, out var result))
        {
            return result;
        }

        throw new ArgumentException($"Не найден текст для операции {type}");
    }
}