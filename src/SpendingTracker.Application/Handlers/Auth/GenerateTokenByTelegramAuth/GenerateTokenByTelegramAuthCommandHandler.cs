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
    private readonly ITelegramHashValidator _telegramHashValidator;

    public GenerateTokenByTelegramAuthCommandHandler(
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository,
        IMediator mediator,
        ITelegramHashValidator telegramHashValidator)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _mediator = mediator;
        _telegramHashValidator = telegramHashValidator;
    }

    public override async Task<GenerateTokenByTelegramAuthResponse> Handle(
        GenerateTokenByTelegramAuthCommand command,
        CancellationToken cancellationToken)
    {
        var hashIsValid = _telegramHashValidator.IsValid(
            command.Hash,
            command.AuthDateAsString,
            command.FirstName,
            command.LastName,
            command.UserId,
            command.PhotoUrl,   
            command.UserName);

        if (!hashIsValid)
        {
            throw new AuthenticationException("Incorrect hash");
        }
        
        var id = await _userRepository.FindIdByTelegramId(command.UserId, cancellationToken);
        if (id is null)
        {
            id = await _mediator.SendCommandAsync<CreateUserByTelegramCommand, UserKey>(new CreateUserByTelegramCommand
            {
                TelegramUserId = command.UserId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.UserName,
                PhotoUrl = command.PhotoUrl
            }, cancellationToken);
        }
    
        var tokenInformation = _tokenGenerator.Create(id.Value);

        return new GenerateTokenByTelegramAuthResponse
        {
            TokenInformation = tokenInformation,
            Id = id,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PhotoUrl = command.PhotoUrl
        };
    }
}