using SpendingTracker.Application.Handlers.Common;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Domain;

namespace SpendingTracker.Application.Handlers.Category.AddExistCategoryAsChildren.Contracts
{
    public class AddExistCategoryAsChildrenCommand : ISpendingTrackerCommand
    {
        public UserKey UserId { get; set; }
        public Guid ParentId { get; set; }
        public Guid ChildId { get; set; }
        public ActionSource ActionSource { get; init; }
    }
}