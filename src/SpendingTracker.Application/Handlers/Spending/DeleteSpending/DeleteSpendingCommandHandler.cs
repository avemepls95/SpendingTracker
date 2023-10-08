using SpendingTracker.Application.Handlers.Spending.DeleteSpending.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.DeleteSpending;

internal sealed class DeleteSpendingCommandHandler : CommandHandler<DeleteSpendingCommand>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSpendingCommandHandler(ISpendingRepository spendingRepository, IUnitOfWork unitOfWork)
    {
        _spendingRepository = spendingRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(DeleteSpendingCommand command, CancellationToken cancellationToken)
    {
        await _spendingRepository.DeleteSpending(command.Id, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}