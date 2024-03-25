namespace SpendingTracker.TelegramBot.TextMessageParsing.Internal;

internal class TextMessagePartParseResult<T>
{
    public T? Result { get; private init; }
    public bool IsSuccess { get; private init; }
    public string? ErrorMessage { get; private init; }

    public static TextMessagePartParseResult<T> Success(T result)
    {
        return new TextMessagePartParseResult<T>
        {
            Result = result,
            IsSuccess = true
        };
    }
    
    public static TextMessagePartParseResult<T> Error(string errorMessage)
    {
        return new TextMessagePartParseResult<T>
        {
            ErrorMessage = errorMessage,
            IsSuccess = false
        };
    }
}