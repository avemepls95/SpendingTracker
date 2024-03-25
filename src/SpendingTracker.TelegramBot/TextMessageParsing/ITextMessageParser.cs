namespace SpendingTracker.TelegramBot.TextMessageParsing;

public interface ITextMessageParser
{
    SpendingMessageParsingResult ParseSpending(string message);
    IncomeMessageParsingResult ParseIncome(string message);
}