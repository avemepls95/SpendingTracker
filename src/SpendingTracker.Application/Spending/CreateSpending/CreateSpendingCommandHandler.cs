using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Spending.CreateSpending;

internal class CreateSpendingCommandHandler : CommandHandler<CreateSpendingCommand>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpendingCommandHandler(ISpendingRepository spendingRepository, IUnitOfWork unitOfWork)
    {
        _spendingRepository = spendingRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(CreateSpendingCommand command, CancellationToken cancellationToken)
    {
        var spending = new Domain.Spending(
            Guid.NewGuid(),
            command.Amount,
            command.User.Currency,
            command.Date,
            command.Description,
            command.ActionSource);

        await _spendingRepository.CreateAsync(spending, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}