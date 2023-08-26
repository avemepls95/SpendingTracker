using SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Contracts;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.User.CreateUserByTelegramId;

internal class CreateUserByTelegramCommandHandler : CommandHandler<CreateUserByTelegramCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserByTelegramCommandHandler(
        IUserRepository userRepository,
        ICurrencyRepository currencyRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _currencyRepository = currencyRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(CreateUserByTelegramCommand command, CancellationToken cancellationToken)
    {
        var defaultCurrency = await _currencyRepository.GetDefaultAsync(cancellationToken);
        var userId = new UserKey(Guid.NewGuid());
        var user = new Domain.User(userId, defaultCurrency)
        {
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        await _userRepository.Create(user, cancellationToken);
        // await _unitOfWork.SaveAsync(cancellationToken);

        await _userRepository.CreateTelegramUser(
            command.TelegramUserId,
            command.LastName,
            command.FirstName,
            command.UserName,
            user.Id,
            cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}