﻿using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;

namespace SpendingTracker.Infrastructure.Factories.Abstractions;

internal interface IUserFactory
{
    User Create(StoredUser user);
}