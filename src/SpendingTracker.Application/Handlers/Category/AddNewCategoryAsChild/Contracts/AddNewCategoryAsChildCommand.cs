using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Category.AddNewCategoryAsChild.Contracts;

public class AddNewCategoryAsChildCommand : ICommand
{
    public UserKey UserId { get; set; }
    public Guid ParentId { get; set; }
    public string ChildTitle { get; set; }
}