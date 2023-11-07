using SpendingTracker.Application.Handlers.Category.RemoveChildCategoryFromParent.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.RemoveChildCategoryFromParent;

internal class RemoveChildCategoryFromParentCommandHandler : CommandHandler<RemoveChildCategoryFromParentCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveChildCategoryFromParentCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(RemoveChildCategoryFromParentCommand command, CancellationToken cancellationToken)
    {
        await _categoryRepository.RemoveChildFromParent(command.ChildId, command.ParentId, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}