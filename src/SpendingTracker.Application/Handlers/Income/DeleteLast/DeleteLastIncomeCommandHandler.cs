using SpendingTracker.Application.Handlers.Income.DeleteLast.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Income.DeleteLast;

internal sealed class DeleteLastIncomeCommandHandler : CommandHandler<DeleteLastIncomeCommand>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLastIncomeCommandHandler(IIncomeRepository incomeRepository, IUnitOfWork unitOfWork)
    {
        _incomeRepository = incomeRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(DeleteLastIncomeCommand command, CancellationToken cancellationToken)
    {
        await _incomeRepository.DeleteLast(command.UserId, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}