using SpendingTracker.Application.Handlers.Category.AddExistCategoryAsChild.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.AddExistCategoryAsChild;

internal class AddExistCategoryAsChildCommandHandler : CommandHandler<AddExistCategoryAsChildCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddExistCategoryAsChildCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(AddExistCategoryAsChildCommand command, CancellationToken cancellationToken)
    {
        await _categoryRepository.AddChildCategory(command.ParentId, command.ChildId, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}