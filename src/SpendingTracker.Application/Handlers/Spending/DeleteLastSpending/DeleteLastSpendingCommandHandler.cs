using SpendingTracker.Application.Handlers.Spending.DeleteLastSpending.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.DeleteLastSpending;

internal class DeleteLastSpendingCommandHandler : CommandHandler<DeleteLastSpendingCommand>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLastSpendingCommandHandler(ISpendingRepository spendingRepository, IUnitOfWork unitOfWork)
    {
        _spendingRepository = spendingRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(DeleteLastSpendingCommand command, CancellationToken cancellationToken)
    {
        await _spendingRepository.DeleteLastUserSpending(command.UserId, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}