namespace SpendingTracker.Application.Handlers.Spending.Services.Abstractions;

internal interface ISpendingsConvertingService
{
    Task<Dictionary<Domain.Spending, double>> GetSpendingsConvertedAmountDict(
        Domain.Spending[] spendings,
        Guid targetCurrencyId,
        CancellationToken cancellationToken = default);
}