using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Category.AddExistCategoriesAsChildrenCommand.Contracts;

public class AddExistCategoriesAsChildrenCommand : ICommand
{
    public Guid ParentId { get; set; }
    public Guid[] ChildIds { get; set; }
}