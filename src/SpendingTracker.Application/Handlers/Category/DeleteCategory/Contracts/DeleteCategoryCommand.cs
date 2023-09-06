using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;

namespace SpendingTracker.Application.Handlers.Category.DeleteCategory.Contracts;

public class DeleteCategoryCommand : ICommand
{
    public UserKey InitiatorId { get; set; }
    public Guid Id { get; set; }
}