using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;

namespace SpendingTracker.Application.Spending.CreateSpending;

public class CreateSpendingCommandHandler : CommandHandler<CreateSpendingCommand>
{
    public override Task Handle(CreateSpendingCommand command, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}