using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Account.CreateAccount.Contracts;
using SpendingTracker.Application.Handlers.Account.GetAccountsInfo.Contracts;
using SpendingTracker.Application.Handlers.Account.UpdateAccount.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.WebApp.Contracts;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/account")]
[Authorize(AuthenticationSchemes = BearerAuth.SchemeName)]
public class AccountController : BaseController
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task Create([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand
        {
            UserId = GetCurrentUserId(),
            Type = request.Type,
            Name = request.Name,
            CurrencyId = request.CurrencyId,
            Amount = request.Amount
        };

        await _mediator.SendCommandAsync(command, cancellationToken);
    }

    [HttpPost("update")]
    public async Task Update([FromBody] UpdateAccountRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateAccountCommand
        {
            Id = request.Id,
            Type = request.Type,
            Name = request.Name,
            CurrencyId = request.CurrencyId,
            Amount = request.Amount
        };

        await _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpGet("get-list-info")]
    public async Task<GetAccountsInfoResponse> GetAccountsInfo(
        [FromQuery] Guid currencyId,
        CancellationToken cancellationToken)
    {
        var query = new GetAccountsInfoQuery
        {
            UserId = GetCurrentUserId(),
            CurrencyId = currencyId
        };

        var result = await _mediator.SendQueryAsync<GetAccountsInfoQuery, GetAccountsInfoResponse>(
            query,
            cancellationToken);

        return result;
    }
}