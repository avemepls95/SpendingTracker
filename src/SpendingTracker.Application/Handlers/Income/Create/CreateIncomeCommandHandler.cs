using SpendingTracker.Application.Handlers.Income.Create.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Models.Request;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Income.Create;

internal sealed class CreateIncomeCommandHandler : CommandHandler<CreateIncomeCommand>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountRepository _accountRepository;

    public CreateIncomeCommandHandler(
        IIncomeRepository incomeRepository,
        IUnitOfWork unitOfWork,
        IAccountRepository accountRepository)
    {
        _incomeRepository = incomeRepository;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
    }

    public override async Task Handle(CreateIncomeCommand command, CancellationToken cancellationToken)
    {
        await _incomeRepository.Create(new CreateIncomeRequest
        {
            Amount = command.Amount,
            Date = command.Date,
            Description = command.Description,
            AccountId = command.AccountId
        }, cancellationToken);

        if (command.AccountId.HasValue)
        {
            await _accountRepository.ChangeAmount(command.AccountId.Value, command.Amount, cancellationToken);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}