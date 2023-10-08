namespace SpendingTracker.Application.Handlers.Currencies.GetAllCurrencies.Contracts;

public class GetAllCurrenciesResponseItem
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string FlagEmojiCode { get; set; }

    public string Title { get; set; }
}