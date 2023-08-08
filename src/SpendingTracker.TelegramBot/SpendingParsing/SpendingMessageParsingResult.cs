namespace SpendingTracker.TelegramBot.SpendingParsing;

public class SpendingMessageParsingResult
{
    public double Amount { get; }
    public DateTimeOffset? Date { get; }
    public string Description { get; }

    public string ErrorMessage { get; set; }

    public SpendingMessageParsingResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public SpendingMessageParsingResult(double amount, string description, DateTimeOffset? date = null)
    {
        Amount = amount;
        Description = description;
        Date = date;
    }
}