using SpendingTracker.Application.Factories.Abstractions;
using SpendingTracker.Application.Handlers.Category.CreateCategory.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.CreateCategory;

internal sealed class CreateCategoryCommandHandler : CommandHandler<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryFactory _categoryFactory;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        ICategoryFactory categoryFactory,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _categoryFactory = categoryFactory;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var userAlreadyHasByTitle = await _categoryRepository.UserHasByTitle(
            command.UserId,
            command.Title.Trim(),
            cancellationToken);

        if (userAlreadyHasByTitle)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.UserAlreadyHasCategoryWithSpecifiedName);
        }
        
        var category = _categoryFactory.Create(command.Title.Trim(), command.UserId);
        await _categoryRepository.CreateCategory(category, cancellationToken);
        
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}