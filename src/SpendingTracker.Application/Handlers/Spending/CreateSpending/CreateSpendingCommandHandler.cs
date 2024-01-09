using SpendingTracker.Application.Handlers.Spending.CreateSpending.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Application.Handlers.Spending.CreateSpending;

internal class CreateSpendingCommandHandler : CommandHandler<CreateSpendingCommand>
{
    private readonly ISpendingRepository _spendingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpendingCommandHandler(
        ISpendingRepository spendingRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository)
    {
        _spendingRepository = spendingRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public override async Task Handle(CreateSpendingCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(command.UserId, cancellationToken);
        
        var spending = new CreateSpendingModel
        {
            Amount = command.Amount,
            CurrencyId = user.Currency.Id,
            Date = command.Date.ToUniversalTime(),
            Description = command.Description,
            ActionSource = command.ActionSource
        };

        await _spendingRepository.CreateAsync(spending, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}