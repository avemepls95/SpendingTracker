using SpendingTracker.TelegramBot.SpendingParsing.Internal;

namespace SpendingTracker.TelegramBot.SpendingParsing;

public class SpendingMessageParser : ISpendingMessageParser
{
    public SpendingMessageParsingResult Parse(string message)
    {
        var lines = message.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
        {
            lines = message.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
            {
                return SpendingMessageParsingResult.Fail(
                    "Недостаточно данных. Как минимум, должны быть указаны сумма и описание траты на разных строках");
            }
        }

        if (lines.Length > 3)
        {
            return SpendingMessageParsingResult.Fail("Слишком много строк с данными.");
        }

        var amountLine = lines[0];
        var amountParseResult = SpendingMessagePartParser.ParseAmount(amountLine);
        if (!amountParseResult.IsSuccess)
        {
            return SpendingMessageParsingResult.Fail(amountParseResult.ErrorMessage);
        }
        
        var descriptionLine = lines[1];
        var descriptionParseResult = SpendingMessagePartParser.ParseDescription(descriptionLine);
        if (!descriptionParseResult.IsSuccess)
        {
            return SpendingMessageParsingResult.Fail(descriptionParseResult.ErrorMessage);
        }

        if (lines.Length == 2)
        {
            return SpendingMessageParsingResult.Success(amountParseResult.Result, descriptionParseResult.Result);
        }
        
        var dateLine = lines[2];
        var dateParseResult = SpendingMessagePartParser.ParseDate(dateLine);
        if (!dateParseResult.IsSuccess)
        {
            return SpendingMessageParsingResult.Fail(dateParseResult.ErrorMessage);
        }

        return SpendingMessageParsingResult.Success(
            amountParseResult.Result,
            descriptionParseResult.Result,
            dateParseResult.Result);
    }
}