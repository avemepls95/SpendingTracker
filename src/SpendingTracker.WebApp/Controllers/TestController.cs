using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Currencies.GetAllCurrencies.Contracts;
using SpendingTracker.Dispatcher.Extensions;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
public class TestController : BaseController
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("test")]
    public async Task<GetAllCurrenciesResponseItem[]> Get(CancellationToken cancellationToken)
    {
        var query = new GetAllCurrenciesQuery();

        var result = await _mediator.SendQueryAsync<GetAllCurrenciesQuery, GetAllCurrenciesResponseItem[]>(
            query,
            cancellationToken);
        
        return result;
    }
}