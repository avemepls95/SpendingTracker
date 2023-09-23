using SpendingTracker.Application.Handlers.UserCurrency.ChangeUserCurrency.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.UserCurrency.ChangeUserCurrency;

internal sealed class ChangeUserCurrencyCommandHandler : CommandHandler<ChangeUserCurrencyCommand>
{
    private readonly IUserCurrencyRepository _userCurrencyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeUserCurrencyCommandHandler(IUserCurrencyRepository userCurrencyRepository, IUnitOfWork unitOfWork)
    {
        _userCurrencyRepository = userCurrencyRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(ChangeUserCurrencyCommand command, CancellationToken cancellationToken)
    {
        await _userCurrencyRepository.ChangeUserCurrency(command.UserId, command.CurrencyCode, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}