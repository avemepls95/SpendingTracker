using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Contracts;
using SpendingTracker.Application.Handlers.Auth.RefreshTokenByTelegramAuth.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.WebApp.Contracts.RefreshAuthToken;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;
 
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("token/generate/telegram")]
    public async Task<GenerateTokenByTelegramAuthResponse> GenerateTokenByTelegramAuth(
        [FromBody] GenerateTokenByTelegramAuthCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.SendCommandAsync<GenerateTokenByTelegramAuthCommand, GenerateTokenByTelegramAuthResponse>(
            command,
            cancellationToken);

        return result;
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = BearerAuth.SchemeName)]
    [Route("token/refresh")]
    public async Task<RefreshTokenByTelegramAuthResponse> RefreshTokenAsync(
        [FromBody] RefreshAuthTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokenByTelegramAuthCommand
        {
            RefreshToken = request.RefreshToken,
            UserId = GetCurrentUserId()
        };

        var result = await _mediator.SendCommandAsync<RefreshTokenByTelegramAuthCommand, RefreshTokenByTelegramAuthResponse>(
            command,
            cancellationToken);

        return result;
    }
}