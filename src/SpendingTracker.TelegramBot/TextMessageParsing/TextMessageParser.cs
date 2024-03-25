using SpendingTracker.TelegramBot.TextMessageParsing.Internal;

namespace SpendingTracker.TelegramBot.TextMessageParsing;

public class TextMessageParser : ITextMessageParser
{
    public SpendingMessageParsingResult ParseSpending(string message)
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
        var amountParseResult = TextMessagePartParser.ParseAmount(amountLine);
        if (!amountParseResult.IsSuccess)
        {
            return SpendingMessageParsingResult.Fail(amountParseResult.ErrorMessage);
        }
        
        var descriptionLine = lines[1];
        var descriptionParseResult = TextMessagePartParser.ParseDescription(descriptionLine);
        if (!descriptionParseResult.IsSuccess)
        {
            return SpendingMessageParsingResult.Fail(descriptionParseResult.ErrorMessage);
        }

        if (lines.Length == 2)
        {
            return SpendingMessageParsingResult.Success(amountParseResult.Result, descriptionParseResult.Result);
        }
        
        var dateLine = lines[2];
        var dateParseResult = TextMessagePartParser.ParseDate(dateLine);
        if (!dateParseResult.IsSuccess)
        {
            return SpendingMessageParsingResult.Fail(dateParseResult.ErrorMessage);
        }

        return SpendingMessageParsingResult.Success(
            amountParseResult.Result,
            descriptionParseResult.Result,
            dateParseResult.Result);
    }

    public IncomeMessageParsingResult ParseIncome(string message)
    {
        var lines = message.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
        {
            lines = message.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
            {
                return IncomeMessageParsingResult.Fail(
                    "Недостаточно данных. Как минимум, должны быть указаны сумма и описание дохода на разных строках");
            }
        }

        if (lines.Length > 4)
        {
            return IncomeMessageParsingResult.Fail("Слишком много строк с данными.");
        }

        var amountLine = lines[0];
        var amountParseResult = TextMessagePartParser.ParseAmount(amountLine);
        if (!amountParseResult.IsSuccess)
        {
            return IncomeMessageParsingResult.Fail(amountParseResult.ErrorMessage);
        }
        
        var descriptionLine = lines[1];
        var descriptionParseResult = TextMessagePartParser.ParseDescription(descriptionLine);
        if (!descriptionParseResult.IsSuccess)
        {
            return IncomeMessageParsingResult.Fail(descriptionParseResult.ErrorMessage);
        }

        if (lines.Length == 2)
        {
            return IncomeMessageParsingResult.Success(amountParseResult.Result, descriptionParseResult.Result);
        }
        
        string accountIndexLine;
        TextMessagePartParseResult<int> accountIndexParseResult;
        var dateLine = lines[2];
        var dateParseResult = TextMessagePartParser.ParseDate(dateLine);
        if (!dateParseResult.IsSuccess)
        {
            accountIndexLine = lines[2];
            accountIndexParseResult = TextMessagePartParser.ParseAccountIndex(accountIndexLine);
            if (!accountIndexParseResult.IsSuccess)
            {
                return IncomeMessageParsingResult.Fail("Не удалось распознать третью строку. Это должна быть дата " +
                                                       "либо индекс счета (если есть хотя бы один счет)");
            }

            if (lines.Length == 3)
            {
                return IncomeMessageParsingResult.Success(
                    amountParseResult.Result,
                    descriptionParseResult.Result,
                    accountIndex: accountIndexParseResult.Result);
            }
            
            dateLine = lines[3];
            dateParseResult = TextMessagePartParser.ParseDate(dateLine);
            if (!dateParseResult.IsSuccess)
            {
                return IncomeMessageParsingResult.Fail(dateParseResult.ErrorMessage);
            }

            return IncomeMessageParsingResult.Success(
                amountParseResult.Result,
                descriptionParseResult.Result,
                dateParseResult.Result,
                accountIndex: accountIndexParseResult.Result);
        }
        
        if (lines.Length == 3)
        {
            return IncomeMessageParsingResult.Success(
                amountParseResult.Result,
                descriptionParseResult.Result,
                date: dateParseResult.Result);
        }
        
        accountIndexLine = lines[3];
        accountIndexParseResult = TextMessagePartParser.ParseAccountIndex(accountIndexLine);
        if (!accountIndexParseResult.IsSuccess)
        {
            return IncomeMessageParsingResult.Fail(accountIndexParseResult.ErrorMessage);
        }

        return IncomeMessageParsingResult.Success(
            amountParseResult.Result,
            descriptionParseResult.Result,
            dateParseResult.Result,
            accountIndex: accountIndexParseResult.Result);
    }
}