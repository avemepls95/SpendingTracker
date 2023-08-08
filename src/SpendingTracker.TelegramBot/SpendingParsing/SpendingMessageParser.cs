namespace SpendingTracker.TelegramBot.SpendingParsing;

public class SpendingMessageParser : ISpendingMessageParser
{
    public bool TryParse(string message, out SpendingMessageParsingResult result)
    {
        var lines = message.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
        {
            result = new SpendingMessageParsingResult("Недостаточно данных. Проверьте корректность указанных данных");
            return false;
        }

        var dateLines = lines.Where(l => DateTimeOffset.TryParse(l, out _)).ToArray();
        if (dateLines.Length > 1)
        {
            result = new SpendingMessageParsingResult("Количество дат больше одной. Требуется одна дата в качестве даты траты");
            return false;
        }

        var amountLines = lines.Where(l => double.TryParse(l, out _)).ToArray();
        if (amountLines.Length != 1)
        {
            result = new SpendingMessageParsingResult("Некорректное количество сумм. Числовое значение суммы траты должно быть одно");
            return false;
        }

        var descriptionLines = lines
            .Where(l =>
                !double.TryParse(l, out _)
                && !float.TryParse(l, out _)
                && !DateTimeOffset.TryParse(l, out _)
                && !TimeSpan.TryParse(l, out _))
            .ToArray();
        if (descriptionLines.Length != 1)
        {
            result = new SpendingMessageParsingResult("Некорректное количество описаний. Описание траты должно быть одно (на одной строке)");
            return false;
        }

        var date = dateLines.Any()
            ? DateTimeOffset.Parse(dateLines.First())
            : null as DateTimeOffset?;

        var amount = double.Parse(amountLines.First());
        var description = descriptionLines.First();

        result = new SpendingMessageParsingResult(amount, description, date);
        return true;
    }
}