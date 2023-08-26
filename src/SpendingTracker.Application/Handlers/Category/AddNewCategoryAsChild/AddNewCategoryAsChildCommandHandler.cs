using SpendingTracker.Application.Factories.Abstractions;
using SpendingTracker.Application.Handlers.Category.AddNewCategoryAsChild.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.AddNewCategoryAsChild;

internal class AddNewCategoryAsChildCommandHandler : CommandHandler<AddNewCategoryAsChildCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryFactory _categoryFactory;
    private readonly IUnitOfWork _unitOfWork;

    public AddNewCategoryAsChildCommandHandler(
        ICategoryRepository categoryRepository,
        ICategoryFactory categoryFactory,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _categoryFactory = categoryFactory;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(AddNewCategoryAsChildCommand command, CancellationToken cancellationToken)
    {
        var newChild = _categoryFactory.Create(command.ChildTitle, command.UserId);
        await _categoryRepository.CreateCategory(newChild, cancellationToken);
        await _categoryRepository.AddChildCategory(command.ParentId, newChild.Id, cancellationToken);

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}