using SpendingTracker.Application.Handlers.Spending.AddSpendingToExistCategory.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.AddSpendingToExistCategory;

internal class AddSpendingToExistCategoryCommandHandler : CommandHandler<AddSpendingToExistCategoryCommand>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public AddSpendingToExistCategoryCommandHandler(
        ISpendingRepository spendingRepository,
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository)
    {
        _spendingRepository = spendingRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public override async Task Handle(AddSpendingToExistCategoryCommand command, CancellationToken cancellationToken)
    {
        var spendingAlreadyHasCategory = await _spendingRepository.IsSpendingHasCategory(
            command.SpendingId,
            command.CategoryId,
            cancellationToken);

        if (spendingAlreadyHasCategory)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.CategoriesAlreadyLinked);
        }

        var userHasCategory = await _categoryRepository.UserHasById(
            command.UserId,
            command.CategoryId,
            cancellationToken);

        if (!userHasCategory)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.CategoryDoesNotBelongsToUser);
        }
        
        await _spendingRepository.AddToCategory(command.SpendingId, command.CategoryId, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}