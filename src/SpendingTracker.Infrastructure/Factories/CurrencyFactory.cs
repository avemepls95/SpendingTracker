﻿using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Factories.Abstractions;

namespace SpendingTracker.Infrastructure.Factories;

internal class CurrencyFactory : ICurrencyFactory
{
    public Currency Create(StoredCurrency storedCurrency)
    {
        return new Currency
        {
            Id = storedCurrency.Id,
            Code = storedCurrency.Code,
            Title = storedCurrency.Title,
            CountryIcon = storedCurrency.CountryIcon,
            IsDefault = storedCurrency.IsDefault
        };
    }
}