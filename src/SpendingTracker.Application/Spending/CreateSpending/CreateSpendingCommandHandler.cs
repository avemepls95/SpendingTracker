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
        var spending = new Domain.Spending
        {
            Id = Guid.NewGuid(),
            Amount = command.Amount,
            Currency = command.User.Currency,
            Date = command.Date,
            Description = command.Description,
            ActionSource = command.ActionSource
        };
        await _spendingRepository.CreateAsync(spending, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}