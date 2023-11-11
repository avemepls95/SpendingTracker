using SpendingTracker.Application.Handlers.Category.DeleteCategory.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.DeleteCategory;

internal sealed class DeleteCategoryCommandHandler : CommandHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetById(command.Id, cancellationToken);
        if (category.OwnerId != command.InitiatorId)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.CurrentUserHasNoPermissionToDeleteCategory);
        }

        await _categoryRepository.DeleteCategory(category, cancellationToken);
        
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}