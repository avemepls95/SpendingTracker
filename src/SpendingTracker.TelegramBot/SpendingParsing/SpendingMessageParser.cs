using System.Globalization;

namespace SpendingTracker.TelegramBot.SpendingParsing;

public class SpendingMessageParser : ISpendingMessageParser
{
    public bool TryParse(string message, out SpendingMessageParsingResult result)
    {
        var lines = message.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
        {
            lines = message.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
            {
                result = new SpendingMessageParsingResult("Недостаточно данных. Проверьте корректность указанных данных");
                return false;    
            }
        }

        var dateFormats = new[]
        {
            "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "dd/MM/yyyy",
            "d/M/yy", "dd/M/yy", "d/MM/yy", "dd/MM/yy",
            "d.M.yyyy", "dd.M.yyyy", "d.MM.yyyy", "dd.MM.yyyy",
            "d.M.yy", "dd.M.yy", "d.MM.yy", "dd.MM.yy"
        };
        var dateLines = lines.Where(l => DateTimeOffset.TryParseExact(l, dateFormats, null, DateTimeStyles.None, out _)).ToArray();
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
                && !DateTimeOffset.TryParseExact(l, dateFormats, null, DateTimeStyles.None, out _)
                && !TimeSpan.TryParse(l, out _))
            .ToArray();
        if (descriptionLines.Length != 1)
        {
            result = new SpendingMessageParsingResult("Некорректное количество описаний. Описание траты должно быть одно (на одной строке)");
            return false;
        }

        var date = dateLines.Any()
            ? DateTimeOffset.ParseExact(dateLines.First(), dateFormats, null, DateTimeStyles.None).ToUniversalTime()
            : null as DateTimeOffset?;

        var amount = double.Parse(amountLines.First());
        var description = descriptionLines.First();

        result = new SpendingMessageParsingResult(amount, description, date);
        return true;
    }
}