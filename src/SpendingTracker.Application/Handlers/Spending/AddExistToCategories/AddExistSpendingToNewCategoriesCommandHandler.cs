using SpendingTracker.Application.Factories.Abstractions;
using SpendingTracker.Application.Handlers.Spending.AddExistToCategories.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.AddExistToCategories;

internal class AddExistSpendingToNewCategoriesCommandHandler : CommandHandler<AddExistSpendingToNewCategoriesCommand>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryFactory _categoryFactory;
    private readonly IUnitOfWork _unitOfWork;

    public AddExistSpendingToNewCategoriesCommandHandler(
        ISpendingRepository spendingRepository,
        ICategoryRepository categoryRepository,
        ICategoryFactory categoryFactory,
        IUnitOfWork unitOfWork)
    {
        _spendingRepository = spendingRepository;
        _categoryRepository = categoryRepository;
        _categoryFactory = categoryFactory;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(
        AddExistSpendingToNewCategoriesCommand command,
        CancellationToken cancellationToken)
    {
        var newCategories = command.CategoryTitles
            .Select(t => _categoryFactory.Create(t, command.UserId))
            .ToArray();

        await _categoryRepository.CreateCategories(newCategories, cancellationToken);
        await _spendingRepository.AddExistToCategories(command.SpendingId, newCategories, cancellationToken);

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}