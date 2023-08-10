namespace SpendingTracker.TelegramBot.Services.Model;

public class CreateSpendingRequest
{
    public double Amount { get; set; }

    public long TelegramUserId { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Description { get; set; }
}