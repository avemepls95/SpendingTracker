using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Category.RemoveChildCategoryFromParent.Contracts;

public class RemoveChildCategoryFromParentCommand : ICommand
{
    public Guid ChildId { get; set; }
    public Guid ParentId { get; set; }
}