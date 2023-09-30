using System.Globalization;

namespace SpendingTracker.TelegramBot.SpendingParsing.Internal;

internal class SpendingMessagePartParser
{
    public static SpendingMessagePartParseResult<DateTimeOffset> ParseDate(string text)
    {
        var dateFormats = new[]
        {
            "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "dd/MM/yyyy",
            "d/M/yy", "dd/M/yy", "d/MM/yy", "dd/MM/yy",

            "d.M.yyyy", "dd.M.yyyy", "d.MM.yyyy", "dd.MM.yyyy",
            "d.M.yy", "dd.M.yy", "d.MM.yy", "dd.MM.yy",
            
            "d/M", "dd/M", "d/MM", "dd/MM",
            "d/M", "dd/M", "d/MM", "dd/MM",

            "d.M", "dd.M", "d.MM", "dd.MM",
            "d.M", "dd.M", "d.MM", "dd.MM",
        };

        var parseIsSuccess = DateTimeOffset.TryParseExact(
            text,
            dateFormats,
            null,
            DateTimeStyles.None,
            out var parseResult);

        if (!parseIsSuccess)
        {
            return SpendingMessagePartParseResult<DateTimeOffset>.Error(
                @"Не удалось распознать дату. Доступные форматы ввода:
- день/месяц/год
- день/месяц
- день.месяц.год
- день.месяц,
где день и месяц могут быть представлены одной или двумя цифрами, а год - двумя или четырьмя.
Если год отсутствует, будет взят текущий."
            );
        }

        return SpendingMessagePartParseResult<DateTimeOffset>.Success(parseResult.ToUniversalTime());
    }

    public static SpendingMessagePartParseResult<float> ParseAmount(string text)
    {
        var formattedText = text?.Replace(",", ".");
        if (!float.TryParse(formattedText, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
            || float.IsInfinity(result))
        {
            return SpendingMessagePartParseResult<float>.Error("Не удалось распознать сумму. Возможно введено не число," +
                                                               " либо число слишком большое");
        }

        if (result < 1)
        {
            return SpendingMessagePartParseResult<float>.Error("Значение суммы должно быть больше 0");
        }

        return SpendingMessagePartParseResult<float>.Success(result);
    }

    public static SpendingMessagePartParseResult<string> ParseDescription(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return SpendingMessagePartParseResult<string>.Error("Описание траты не может быть пустое");
        }

        return SpendingMessagePartParseResult<string>.Success(text);
    }
}