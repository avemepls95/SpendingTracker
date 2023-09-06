using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Category.GetUserCategories.Contracts;

public class GetUserCategoriesQuery : IQuery<GetUserCategoriesResponseItem[]>
{
    public UserKey UserId { get; set; }
}