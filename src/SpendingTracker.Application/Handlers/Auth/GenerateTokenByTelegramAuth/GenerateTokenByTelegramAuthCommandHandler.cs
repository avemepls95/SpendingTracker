using System.Security.Authentication;
using MediatR;
using SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth._Internal;
using SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Contracts;
using SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Contracts;
using SpendingTracker.BearerTokenAuth.Abstractions;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth;

internal sealed class GenerateTokenByTelegramAuthCommandHandler
    : CommandHandler<GenerateTokenByTelegramAuthCommand, GenerateTokenByTelegramAuthResponse>
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;
    private readonly ITelegramHashValidator _telegramCheckStringValidator;

    public GenerateTokenByTelegramAuthCommandHandler(
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository,
        IMediator mediator,
        ITelegramHashValidator telegramCheckStringValidator)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _mediator = mediator;
        _telegramCheckStringValidator = telegramCheckStringValidator;
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
        
        var id = await _userRepository.FindIdByTelegramId(command.UserId, cancellationToken);
        if (id is null)
        {
            id = await _mediator.SendCommandAsync<CreateUserByTelegramCommand, UserKey>(new CreateUserByTelegramCommand
            {
                TelegramUserId = command.UserId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.UserName
            }, cancellationToken);
        }
    
        var tokenInformation = _tokenGenerator.Create(id.Value);

        return new GenerateTokenByTelegramAuthResponse
        {
            TokenInformation = tokenInformation,
            Id = id,
            FirstName = command.FirstName,
            LastName = command.LastName
        };
    }
}