using SpendingTracker.Application.Handlers.Account.UpdateAccount.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Application.Handlers.Account.UpdateAccount;

internal sealed class CreateAccountCommandHandler : CommandHandler<UpdateAccountCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var isExists = await _accountRepository.IsExistsById(command.Id, cancellationToken);
        if (!isExists)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.KeyNotFound);
        }

        var updateModel = new UpdateAccountModel
        {
            Id = command.Id,
            Type = command.Type,
            Name = command.Name.Trim(),
            CurrencyId = command.CurrencyId,
            Amount = command.Amount
        };

        await _accountRepository.Update(updateModel, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}