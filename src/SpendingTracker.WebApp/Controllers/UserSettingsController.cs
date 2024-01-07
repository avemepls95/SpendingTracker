using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.UserSettings.GetUserSettings.Contracts;
using SpendingTracker.Application.Handlers.UserSettings.UpdateUserSettings.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.WebApp.Contracts.UpdateUserSettings;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/user-settings")]
[Authorize(AuthenticationSchemes = BearerAuth.SchemeName)]
public class UserSettingsController : BaseController
{
    private readonly IMediator _mediator;

    public UserSettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list")]
    public async Task<GetUserSettingsQueryResponse> GetUserSettings(CancellationToken cancellationToken)
    {
        var query = new GetUserSettingsQuery
        {
            UserId = GetCurrentUserId()
        };

        var result = await _mediator.SendQueryAsync<GetUserSettingsQuery, GetUserSettingsQueryResponse>(
            query,
            cancellationToken);

        return result;
    }
    
    [HttpPost("update")]
    public async Task UpdateUserSettings(
        [FromBody] UpdateUserSettingsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserSettingsCommand()
        {
            UserId = GetCurrentUserId(),
            ViewCurrencyId = request.ViewCurrencyId
        };

        await _mediator.SendCommandAsync(command, cancellationToken);
    }
}