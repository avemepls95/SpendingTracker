using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Auth.AuthByTelegram.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Dispatcher.Extensions;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("telegram/get-token")]
    public async Task<AuthByTelegramResponse> AuthByTelegram(
        [FromBody] AuthByTelegramCommand command,
        CancellationToken cancellationToken)
    {
        var tokenInformation = await _mediator.SendCommandAsync<AuthByTelegramCommand, AuthByTelegramResponse>(
            command,
            cancellationToken);

        return tokenInformation;
    }
    //
    // [HttpPost]
    // [Route("telegram/refresh-token")]
    // public Task<TokenInformation> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    // {
    //     var refreshTokenTask = _tokenGenerator.RefreshAsync(
    //         refreshToken,
    //         _basicAuthCredentials.UserName,
    //         _basicAuthCredentials.Password,
    //         cancellationToken);
    //
    //     return refreshTokenTask;
    // }
}