using SpendingTracker.Application.Factories.Abstractions;
using SpendingTracker.Application.Handlers.Category.AddNewCategoryAsParent.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.AddNewCategoryAsParent;

internal class AddNewCategoryAsParentCommandHandler : CommandHandler<AddNewCategoryAsParentCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryFactory _categoryFactory;
    private readonly IUnitOfWork _unitOfWork;

    public AddNewCategoryAsParentCommandHandler(
        ICategoryRepository categoryRepository,
        ICategoryFactory categoryFactory,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _categoryFactory = categoryFactory;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(AddNewCategoryAsParentCommand command, CancellationToken cancellationToken)
    {
        var userAlreadyHasByTitle = await _categoryRepository.UserHasByTitle(
            command.UserId,
            command.NewParentTitle.Trim(),
            cancellationToken);

        if (userAlreadyHasByTitle)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.UserAlreadyHasCategoryWithSpecifiedName);
        }

        var newParent = _categoryFactory.Create(command.NewParentTitle.Trim(), command.UserId);
        await _categoryRepository.CreateCategory(newParent, cancellationToken);
        await _categoryRepository.AddChildCategory(newParent.Id, command.ChildId, cancellationToken);

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}