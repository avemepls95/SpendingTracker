using SpendingTracker.Application.Handlers.Spending.UpdateSpending.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Models.Request;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Spending.UpdateSpending;

internal sealed class UpdateSpendingCommandHandler : CommandHandler<UpdateSpendingCommand>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSpendingCommandHandler(ISpendingRepository spendingRepository, IUnitOfWork unitOfWork)
    {
        _spendingRepository = spendingRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(UpdateSpendingCommand command, CancellationToken cancellationToken)
    {
        var repositoryModel = new UpdateSpendingModel
        {
            Id = command.Id,
            Amount = command.Amount,
            Date = command.Date.ToUniversalTime(),
            CurrencyId = command.CurrencyId,
            Description = command.Description.Trim()
        };

        await _spendingRepository.UpdateSpending(repositoryModel, cancellationToken);

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}