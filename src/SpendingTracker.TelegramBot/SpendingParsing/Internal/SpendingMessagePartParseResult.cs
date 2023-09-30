namespace SpendingTracker.TelegramBot.SpendingParsing.Internal;

internal class SpendingMessagePartParseResult<T>
{
    public T? Result { get; private init; }
    public bool IsSuccess { get; private init; }
    public string? ErrorMessage { get; private init; }

    public static SpendingMessagePartParseResult<T> Success(T result)
    {
        return new SpendingMessagePartParseResult<T>
        {
            Result = result,
            IsSuccess = true
        };
    }
    
    public static SpendingMessagePartParseResult<T> Error(string errorMessage)
    {
        return new SpendingMessagePartParseResult<T>
        {
            ErrorMessage = errorMessage,
            IsSuccess = false
        };
    }
}