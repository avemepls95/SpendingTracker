using SpendingTracker.Application.Factories.Abstractions;
using SpendingTracker.Application.Handlers.Spending.AddSpendingToNewCategory.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.AddSpendingToNewCategory;

internal class AddSpendingToNewCategoryCommandHandler : CommandHandler<AddSpendingToNewCategoryCommand>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryFactory _categoryFactory;

    public AddSpendingToNewCategoryCommandHandler(
        ISpendingRepository spendingRepository,
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        ICategoryFactory categoryFactory)
    {
        _spendingRepository = spendingRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _categoryFactory = categoryFactory;
    }

    public override async Task Handle(AddSpendingToNewCategoryCommand command, CancellationToken cancellationToken)
    {
        var userAlreadyHasByTitle = await _categoryRepository.UserHasByTitle(
            command.UserId,
            command.NewCategoryTitle,
            cancellationToken);

        if (userAlreadyHasByTitle)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.UserAlreadyHasCategoryWithSpecifiedName);
        }
        
        var category = _categoryFactory.Create(command.NewCategoryTitle, command.UserId);
        await _categoryRepository.CreateCategory(category, cancellationToken);

        await _spendingRepository.AddToCategory(command.SpendingId, category.Id, cancellationToken);
        
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}