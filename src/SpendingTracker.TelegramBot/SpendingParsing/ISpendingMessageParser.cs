namespace SpendingTracker.TelegramBot.SpendingParsing;

public interface ISpendingMessageParser
{
    SpendingMessageParsingResult Parse(string message);
}