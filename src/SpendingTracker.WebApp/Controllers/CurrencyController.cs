using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Currencies.GetAllCurrencies.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Dispatcher.Extensions;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/currency")]
[Authorize(AuthenticationSchemes = BearerAuth.SchemeName)]
public class CurrencyController : BaseController
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list")]
    public async Task<GetAllCurrenciesResponseItem[]> Get(CancellationToken cancellationToken)
    {
        var query = new GetAllCurrenciesQuery();

        var result = await _mediator.SendQueryAsync<GetAllCurrenciesQuery, GetAllCurrenciesResponseItem[]>(
            query,
            cancellationToken);
        
        return result;
    }
}