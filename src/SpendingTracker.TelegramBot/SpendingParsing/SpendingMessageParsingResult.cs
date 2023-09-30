namespace SpendingTracker.TelegramBot.SpendingParsing;

public class SpendingMessageParsingResult
{
    public bool IsSuccess { get; init; }
    public double Amount { get; private init; }
    public DateTimeOffset? Date { get; private init; }
    public string Description { get; private init; }

    public string? ErrorMessage { get; private init; }

    public static SpendingMessageParsingResult Fail(string? errorMessage)
    {
        return new SpendingMessageParsingResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }

    public static SpendingMessageParsingResult Success(double amount, string description, DateTimeOffset? date = null)
    {
        return new SpendingMessageParsingResult
        {
            IsSuccess = true,
            Amount = amount,
            Description = description,
            Date = date
        };
    }
}