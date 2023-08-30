namespace SpendingTracker.TelegramBot.Internal.Buttons;

public class ButtonsGroupTextProvider
{
    private static readonly Dictionary<ButtonsGroupOperation, string> OperationTextDict = new()
    {
        { ButtonsGroupOperation.CreateSpending, @"Введите трату в формате

Cумма
Описание
Дата (опционально)" }
    };

    public static string GetText(ButtonsGroupOperation operation)
    {
        if (OperationTextDict.TryGetValue(operation, out var result))
        {
            return result;
        }

        throw new ArgumentException($"Не найден текст для операции {operation}");
    }
}