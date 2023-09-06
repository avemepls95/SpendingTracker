using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Category.CreateCategory.Contracts;

public class CreateCategoryCommand : ICommand
{
    public UserKey UserId { get; set; }
    public string Title { get; set; }
}