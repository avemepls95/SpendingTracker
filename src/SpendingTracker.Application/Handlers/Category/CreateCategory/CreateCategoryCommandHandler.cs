using SpendingTracker.Application.Factories.Abstractions;
using SpendingTracker.Application.Handlers.Category.CreateCategory.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
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
        var category = _categoryFactory.Create(command.Title, command.UserId);
        await _categoryRepository.CreateCategory(category, cancellationToken);
        
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}