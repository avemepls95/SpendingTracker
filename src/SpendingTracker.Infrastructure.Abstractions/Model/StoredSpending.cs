﻿using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Model.Categories;

namespace SpendingTracker.Infrastructure.Abstractions.Model;

public class StoredSpending : EntityObject<StoredSpending, Guid>
{
    public Guid Id { get; set; }
    public double Amount { get; set; }
    public Guid CurrencyId { get; set; }
    public StoredCurrency Currency { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Description { get; set; }
    public ActionSource ActionSource { get; set; }
    public List<SpendingCategoryLink>? CategoryLinks { get; set; }
    public bool IsDeleted { get; set; }

    public override Guid GetKey()
    {
        return Id;
    }
}