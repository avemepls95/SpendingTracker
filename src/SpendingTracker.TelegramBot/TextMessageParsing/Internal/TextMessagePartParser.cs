using System.Globalization;

namespace SpendingTracker.TelegramBot.TextMessageParsing.Internal;

internal class TextMessagePartParser
{
    public static TextMessagePartParseResult<DateTimeOffset> ParseDate(string text)
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
            return TextMessagePartParseResult<DateTimeOffset>.Error(
                @"Не удалось распознать дату. Доступные форматы ввода:
- день/месяц/год
- день/месяц
- день.месяц.год
- день.месяц,
где день и месяц могут быть представлены одной или двумя цифрами, а год - двумя или четырьмя.
Если год отсутствует, будет взят текущий."
            );
        }

        return TextMessagePartParseResult<DateTimeOffset>.Success(parseResult.ToUniversalTime());
    }

    public static TextMessagePartParseResult<float> ParseAmount(string text)
    {
        var formattedText = text?.Replace(",", ".");
        if (!float.TryParse(formattedText, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
            || float.IsInfinity(result))
        {
            return TextMessagePartParseResult<float>.Error("Не удалось распознать сумму. Возможно введено не число," +
                                                               " либо число слишком большое");
        }

        if (result < 1)
        {
            return TextMessagePartParseResult<float>.Error("Значение суммы должно быть больше 0");
        }

        return TextMessagePartParseResult<float>.Success(result);
    }

    public static TextMessagePartParseResult<string> ParseDescription(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return TextMessagePartParseResult<string>.Error("Описание не может быть пустое");
        }

        return TextMessagePartParseResult<string>.Success(text);
    }
    
    public static TextMessagePartParseResult<int> ParseAccountIndex(string text)
    {
        if (!int.TryParse(text, out var result))
        {
            return TextMessagePartParseResult<int>.Error("Не удалось распознать индекс счета. Возможно введено не число," +
                                                         " либо число слишком большое");
        }

        if (result < 1)
        {
            return TextMessagePartParseResult<int>.Error("Значение индекса должно быть больше 0");
        }

        return TextMessagePartParseResult<int>.Success(result);
    }
}