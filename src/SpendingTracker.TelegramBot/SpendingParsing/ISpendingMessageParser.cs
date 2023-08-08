namespace SpendingTracker.TelegramBot.SpendingParsing;

public interface ISpendingMessageParser
{
    bool TryParse(string message, out SpendingMessageParsingResult? result);
}