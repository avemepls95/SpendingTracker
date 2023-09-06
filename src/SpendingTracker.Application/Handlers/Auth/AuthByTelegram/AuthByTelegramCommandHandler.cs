using MediatR;
using SpendingTracker.Application.Handlers.Auth.AuthByTelegram.Contracts;
using SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.BearerTokenAuth.Abstractions;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Auth.AuthByTelegram;

internal sealed class AuthByTelegramCommandHandler : CommandHandler<AuthByTelegramCommand, AuthByTelegramResponse>
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;

    public AuthByTelegramCommandHandler(
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository,
        IMediator mediator)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _mediator = mediator;
    }

    public override async Task<AuthByTelegramResponse> Handle(
        AuthByTelegramCommand command,
        CancellationToken cancellationToken)
    {
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

        return new AuthByTelegramResponse
        {
            TokenInformation = tokenInformation,
            Id = id,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PhotoUrl = command.PhotoUrl
        };
    }
}