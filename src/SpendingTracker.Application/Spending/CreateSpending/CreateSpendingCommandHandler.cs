using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Spending.CreateSpending;

public class CreateSpendingCommandHandler : CommandHandler<CreateSpendingCommand>
{
    private readonly IUserCurrencyRepository _userCurrencyRepository;

    public CreateSpendingCommandHandler(IUserCurrencyRepository userCurrencyRepository)
    {
        _userCurrencyRepository = userCurrencyRepository;
    }

    public override Task Handle(CreateSpendingCommand command, CancellationToken cancellationToken)
    {
        return _userCurrencyRepository.Get(command.UserKey, cancellationToken);

    }
}