using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;

namespace SpendingTracker.Application.Handlers.Spending.GetSpendings.Converters;

public static class SpendingConverter
{
    public static GetSpendingsResponseItem ConvertToDto(Domain.Spending spending)
    {
        return new GetSpendingsResponseItem
        {
            Id = spending.Id,
            Amount = spending.Amount,
            Currency = new CurrencyDto
            {
                Code = spending.Currency.Code,
                FlagEmojiCode = spending.Currency.CountryIcon,
                Description = spending.Description
            },
            Date = spending.Date,
            Description = spending.Description,
            CreateDate = spending.CreatedDate
        };
    }
}