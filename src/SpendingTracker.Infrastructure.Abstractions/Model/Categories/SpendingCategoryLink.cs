﻿namespace SpendingTracker.Infrastructure.Abstractions.Model.Categories;

public class SpendingCategoryLink
{
    public Guid SpendingId { get; set; }

    public StoredSpending Spending { get; set; }

    public Guid CategoryId { get; set; }

    public StoredCategory Category { get; set; }
}