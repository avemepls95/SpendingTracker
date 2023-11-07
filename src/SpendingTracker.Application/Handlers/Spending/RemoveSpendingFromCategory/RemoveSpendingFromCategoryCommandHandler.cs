using SpendingTracker.Application.Handlers.Spending.RemoveSpendingFromCategory.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.RemoveSpendingFromCategory;

internal class RemoveSpendingFromCategoryCommandHandler : CommandHandler<RemoveSpendingFromCategoryCommand>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveSpendingFromCategoryCommandHandler(ISpendingRepository spendingRepository, IUnitOfWork unitOfWork)
    {
        _spendingRepository = spendingRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(RemoveSpendingFromCategoryCommand command, CancellationToken cancellationToken)
    {
        await _spendingRepository.RemoveFromCategory(command.SpendingId, command.CategoryId, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}