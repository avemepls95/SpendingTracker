using SpendingTracker.Application.Handlers.Account.CreateAccount.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Application.Handlers.Account.CreateAccount;

internal sealed class CreateAccountCommandHandler : CommandHandler<CreateAccountCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var currentCount = await _accountRepository.GetUserAccountsCount(command.UserId, cancellationToken);
        if (currentCount == 20)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.TooManyAccountsCount);
        }
        
        var createModel = new CreateAccountModel
        {
            UserId = command.UserId,
            Type = command.Type,
            Name = command.Name,
            CurrencyId = command.CurrencyId,
            Amount = command.Amount
        };

        await _accountRepository.Create(createModel, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}