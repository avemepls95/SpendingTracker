﻿namespace SpendingTracker.Domain.Constants;

public sealed class SpendingConstants
{
    public const int DescriptionMaxLength = 100;
    public static DateTimeOffset MinDate => DateTimeOffset.Parse("2023-08-13");
}