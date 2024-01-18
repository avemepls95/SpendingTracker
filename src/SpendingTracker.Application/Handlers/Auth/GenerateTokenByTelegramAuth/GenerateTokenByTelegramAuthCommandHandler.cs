using System.Security.Authentication;
using MediatR;
using SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth._Internal;
using SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Contracts;
using SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Contracts;
using SpendingTracker.BearerTokenAuth.Abstractions;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth;

internal sealed class GenerateTokenByTelegramAuthCommandHandler
    : CommandHandler<GenerateTokenByTelegramAuthCommand, GenerateTokenByTelegramAuthResponse>
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;
    private readonly ITelegramHashValidator _telegramCheckStringValidator;
    private readonly IAuthLogRepository _authLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateTokenByTelegramAuthCommandHandler(
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository,
        IMediator mediator,
        ITelegramHashValidator telegramCheckStringValidator,
        IAuthLogRepository authLogRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _mediator = mediator;
        _telegramCheckStringValidator = telegramCheckStringValidator;
        _authLogRepository = authLogRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<GenerateTokenByTelegramAuthResponse> Handle(
        GenerateTokenByTelegramAuthCommand command,
        CancellationToken cancellationToken)
    {
        var checkStringIsValid = _telegramCheckStringValidator.IsValid(command.CheckString, command.AuthType);
        if (!checkStringIsValid)
        {
            throw new AuthenticationException("Incorrect CheckString");
        }
        
        var userId = await _userRepository.FindIdByTelegramId(command.UserId, cancellationToken);
        if (userId is null)
        {
            userId = await _mediator.SendCommandAsync<CreateUserByTelegramCommand, UserKey>(new CreateUserByTelegramCommand
            {
                TelegramUserId = command.UserId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.UserName
            }, cancellationToken);
        }
    
        var tokenInformation = _tokenGenerator.Create(userId.Value);

        await SaveLog(userId, command.AuthType, cancellationToken);

        return new GenerateTokenByTelegramAuthResponse
        {
            TokenInformation = tokenInformation,
            Id = userId,
            FirstName = command.FirstName,
            LastName = command.LastName
        };
    }

    private async Task SaveLog(UserKey userId, TelegramAuthType authType, CancellationToken cancellationToken)
    {
        await _authLogRepository.Create(userId, AuthSource.Telegram, new { Type = authType }, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        // _ = _mediator.SendCommandAsync(new CreateAuthLogCommand
        // {
        //     UserId = userId,
        //     Source = AuthSource.Telegram,
        //     AdditionalData = new { Type = authType }
        // }, cancellationToken);
        // _ = Task.Run(async () =>
        // {
        //     await _mediator.SendCommandAsync(new CreateAuthLogCommand
        //     {
        //         UserId = userId,
        //         Source = AuthSource.Telegram,
        //         AdditionalData = new { Type = authType }
        //     }, cancellationToken);
        // }, cancellationToken);
    }
}