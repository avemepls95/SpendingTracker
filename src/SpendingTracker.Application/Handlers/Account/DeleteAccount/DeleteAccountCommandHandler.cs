using SpendingTracker.Application.Handlers.Account.DeleteAccount.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Account.DeleteAccount;

internal sealed class CreateAccountCommandHandler : CommandHandler<DeleteAccountCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var isExists = await _accountRepository.IsExistsById(command.Id, cancellationToken);
        if (!isExists)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.KeyNotFound);
        }

        await _accountRepository.MarkAsDeleted(command.Id, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}