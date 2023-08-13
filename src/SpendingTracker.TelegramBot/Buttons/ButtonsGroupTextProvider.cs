namespace SpendingTracker.TelegramBot.Buttons;

public class ButtonsGroupTextProvider
{
    private static Dictionary<ButtonsGroupOperation, string> operationTextDict = new()
    {
        { ButtonsGroupOperation.CreateSpending, "Введите трату в формате сумма/дата/описание (каждое значение на новой строке)" }
    };

    public static string GetText(ButtonsGroupOperation operation)
    {
        if (operationTextDict.TryGetValue(operation, out var result))
        {
            return result;
        }

        throw new ArgumentException($"Не найден текст для операции {operation}");
    }
}