namespace SpendingTracker.TelegramBot.TextMessageParsing;

public class IncomeMessageParsingResult
{
    public bool IsSuccess { get; init; }
    public double Amount { get; private init; }
    public string Description { get; private init; }
    public DateTimeOffset? Date { get; private init; }
    public int? AccountIndex { get; private init; }

    public string? ErrorMessage { get; private init; }

    public static IncomeMessageParsingResult Fail(string? errorMessage)
    {
        return new IncomeMessageParsingResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }

    public static IncomeMessageParsingResult Success(
        double amount,
        string description,
        DateTimeOffset? date = null,
        int? accountIndex = null)
    {
        return new IncomeMessageParsingResult
        {
            IsSuccess = true,
            Amount = amount,
            Description = description,
            Date = date,
            AccountIndex = accountIndex
        };
    }
}