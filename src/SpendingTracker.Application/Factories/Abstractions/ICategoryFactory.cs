using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain.Categories;

namespace SpendingTracker.Application.Factories.Abstractions;

internal interface ICategoryFactory
{
    Category Create(string title, UserKey userId);
}